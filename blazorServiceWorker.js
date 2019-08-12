const filesToCache = [
    // Blazor standard requirements
    '_framework/_bin/Microsoft.AspNetCore.Blazor.dll',
    '_framework/_bin/Microsoft.AspNetCore.Metadata.dll',
    '_framework/_bin/Microsoft.AspNetCore.Authorization.dll',
    '_framework/_bin/Microsoft.Extensions.Logging.Abstractions.dll',
    '_framework/_bin/Microsoft.Extensions.Options.dll',
    '_framework/_bin/Microsoft.Extensions.Primitives.dll',
    '_framework/_bin/Microsoft.AspNetCore.Components.dll',
    '_framework/_bin/Microsoft.AspNetCore.Components.Browser.dll',
    '_framework/_bin/Microsoft.Bcl.AsyncInterfaces.dll',
   
    //'/_framework/_bin/Microsoft.AspNetCore.Blazor.TagHelperWorkaround.dll',
    '_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll',
    '_framework/_bin/Microsoft.Extensions.DependencyInjection.dll',
    '_framework/_bin/Microsoft.JSInterop.dll',
    '_framework/_bin/Mono.WebAssembly.Interop.dll',
    '_framework/_bin/Mono.Security.dll',
    '_framework/_bin/mscorlib.dll',
    '_framework/_bin/System.Core.dll',
    '_framework/_bin/System.dll',
    '_framework/_bin/System.Memory.dll',
    '_framework/_bin/System.Numerics.Vectors.dll',
    '_framework/_bin/System.Buffers.dll',
    '_framework/_bin/System.Text.Json.dll ',
    '_framework/_bin/System.Threading.Tasks.Extensions.dll',
    '_framework/_bin/System.Net.Http.dll',
    '_framework/_bin/System.ComponentModel.Annotations.dll',
    '_framework/_bin/System.Runtime.CompilerServices.Unsafe.dll',
    '_framework/wasm/mono.js',
    '_framework/wasm/mono.wasm',
    '_framework/blazor.boot.json',
    '_framework/blazor.webassembly.js',
     
    // App specific requirements
    '_framework/_bin/miNationalrot.dll',

    'css/bootstrap/bootstrap.min.css',
    'css/open-iconic/font/css/open-iconic-bootstrap.min.css',
    'css/site.css',
    'css/open-iconic/font/fonts/open-iconic.woff',
    'icons/icon-192-192.png',
    'icons/icon-512-512.png',
    'favicon.ico',
    'index.html',
 
    // Service Worker
    'blazorSWRegister.js',
 
    // Application Manifest (PWA)
    'manifest.json'
];
 
const staticCacheName = 'blazor-cache-v4';

self.addEventListener('install', event => {
    self.skipWaiting();
    event.waitUntil(
        caches.open(staticCacheName)
            .then(cache => {
                return cache.addAll(filesToCache);
            })
    );
});

self.addEventListener('fetch', event => {
    var requestUrl = new URL(event.request.url);
 
    // First, handle requests for the root path - server up index.html
    if (requestUrl.origin === location.origin) {
        if (requestUrl.pathname === '/') {
            event.respondWith(caches.match('/index.html'));
            return;
        }
    }
    // Anything else
    event.respondWith(
        // Check the cache
        caches.match(event.request)
            .then(response => {
                // anything found in the cache can be returned from there
                // without passing it on to the network
                if (response) {
                    console.log('Found ', event.request.url, ' in cache');
                    return response;
                }
                // otherwise make a network request
                return fetch(event.request)
                    .then(response => {
                        // if we got a valid response 
                        if (response.ok) {
                            // and the request was for something rfom our own app url
                            // we should add it to the cache
                            if (requestUrl.origin === location.origin) {
 
                                const pathname = requestUrl.pathname;
                                console.log("CACHE: Adding " + pathname);
                                return caches.open(staticCacheName).then(cache => {
                                    // you can only "read" a response once, 
                                    // but you can clone it and use that for the cache
                                    cache.put(event.request.url, response.clone());
                                });
                            }
                        }
                        return response;
                    });
            }).catch(error => {
                // handle this error - for now just log it
                console.log(error);
            })
    );
});


var deferredPrompt;

self.addEventListener('beforeinstallprompt', function (e) {
    // Prevent Chrome 67 and earlier from automatically showing the prompt
    e.preventDefault();
    // Stash the event so it can be triggered later.
    deferredPrompt = e;

    showAddToHomeScreen();

});

function showAddToHomeScreen() {

    var a2hsBtn = document.querySelector(".ad2hs-prompt");

    a2hsBtn.style.display = "block";

    a2hsBtn.addEventListener("click", addToHomeScreen);

}

function addToHomeScreen() {
    var a2hsBtn = document.querySelector(".ad2hs-prompt");  // hide our user interface that shows our A2HS button
    a2hsBtn.style.display = 'none';  // Show the prompt
    deferredPrompt.prompt();  // Wait for the user to respond to the prompt
    deferredPrompt.userChoice
        .then(function (choiceResult) {

            if (choiceResult.outcome === 'accepted') {
                console.log('User accepted the A2HS prompt');
            } else {
                console.log('User dismissed the A2HS prompt');
            }

            deferredPrompt = null;

        });
}

//https://mobiforge.com/design-development/pwa-minimus-a-minimal-pwa-checklist
//self.addEventListener('install', function (event) {
//    event.waitUntil(
//        caches.open('sw-cache').then(function (cache) {
//            return cache.add('index.html');
//        })
//    );
//});

//self.addEventListener('fetch', function (event) {
//    event.respondWith(
//        caches.match(event.request).then(function (response) {
//            return response || fetch(event.request);
//        })
//    );
//});