using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.WebHooks.Models;
using OrchardCore.WebHooks.Services;
using OrchardCore.WebHooks.ViewModels;

namespace OrchardCore.WebHooks.Controllers
{
    [Admin]
    public class WebHookController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IWebHookStore _store;
        private readonly IWebHookEventManager _eventManager;
        private readonly INotifier _notifier;

        public WebHookController(
            IWebHookStore store,
            IWebHookEventManager eventManager,
            IAuthorizationService authorizationService,
            INotifier notifier,
            IStringLocalizer<WebHookController> stringLocalizer,
            IHtmlLocalizer<WebHookController> htmlLocalizer,
            ILogger<WebHookController> logger
            )
        {
            _store = store;
            _eventManager = eventManager;
            _authorizationService = authorizationService;
            _notifier = notifier;

            S = stringLocalizer;
            H = htmlLocalizer;
            Logger = logger;
        }

        public IStringLocalizer S { get; set; }

        public IHtmlLocalizer H { get; set; }

        public ILogger<WebHookController> Logger { get; }

        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            var webHooksList = await _store.GetAllWebHooksAsync();

            var model = new WebHookIndexViewModel
            {
                WebHooksList = webHooksList
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            var events = await _eventManager.GetAllWebHookEventsAsync();
            var model = new EditWebHookViewModel
            {
                Events = events
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EditWebHookViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                await ProcessWebHookAsync(model);
                await _store.CreateWebHookAsync(model.WebHook);

                _notifier.Success(H["Webhook created successfully"]);
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            model.Events = await _eventManager.GetAllWebHookEventsAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            var webHook = await _store.GetWebHookAsync(id);

            if (webHook == null)
            {
                return NotFound();
            }

            var events = await _eventManager.GetAllWebHookEventsAsync();
            var model = new EditWebHookViewModel
            {
                Events = events,
                WebHook = webHook,
                CustomPayload = webHook.PayloadTemplate != null,
                SubscribeAllEvents = webHook.Events.Contains("*")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditWebHookViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            var webHook = await _store.GetWebHookAsync(model.WebHook.Id);

            if (webHook == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await ProcessWebHookAsync(model);
                await _store.TryUpdateWebHook(model.WebHook);

                _notifier.Success(H["Webhook updated successfully"]);

                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            model.Events = await _eventManager.GetAllWebHookEventsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            var webHook = await _store.GetWebHookAsync(id);

            if (webHook == null)
            {
                return NotFound();
            }

            await _store.DeleteWebHookAsync(id);

            _notifier.Success(H["Webhook deleted successfully"]);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Ping(string id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageWebHooks))
            {
                return Unauthorized();
            }

            var webHook = await _store.GetWebHookAsync(id);

            if (webHook == null)
            {
                return NotFound();
            }

            var ping = new Ping();
            var uri = new Uri(webHook.Url);

            PingReply result = null;
            string errorMessage = string.Empty;
            try
            {
                result = ping.Send(uri.Host);
            }
            catch (PingException ex)
            {
                errorMessage = ex.Message;
                Logger.LogInformation(ex, "Failed to ping {0} due to failure: {1}.", uri.Host, errorMessage);
            }

            if (result == null || result.Status != IPStatus.Success)
            {
                _notifier.Error(H["Failed to ping {0}. {1}", uri.Host, errorMessage]);
            }
            else
            {
                _notifier.Success(H["Successfully pinged {0}", uri.Host]);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task ProcessWebHookAsync(EditWebHookViewModel model)
        {
            if (model.SubscribeAllEvents)
            {
                model.WebHook.Events.Clear();
            }

            if (!model.CustomPayload)
            {
                model.WebHook.PayloadTemplate = null;
            }

            model.WebHook.Events = await _eventManager.NormalizeEventsAsync(model.WebHook.Events);
        }
    }
}
