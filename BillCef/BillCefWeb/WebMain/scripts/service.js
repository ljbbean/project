cefnet = function () {
    this.ajax = new XMLHttpRequest();
}

cefnet.prototype = {
    _setHeader: function (headers) {
        this.ajax.setRequestHeader("Content-type", "application/json");
        if (!headers) {
            return;
        }
        for (var i in headers) {
            var h = headers[i];
            ajax.setRequestHeader(h, headers[h]);
        }
    },
    post: function (url, data, cb, error, headers) {
        try {
            if (typeof data == 'function') {
                headers = error;
                error = cb;
                cb = data;
                data = {};
            }
            data = JSON.stringify(data);
            this.ajax.open("post", url);
            this._setHeader(headers);
            this.ajax.send(data);
            var _this = this;
            this.ajax.onreadystatechange = function () {
                if (_this.ajax.readyState == 4) {
                    var text = _this.ajax.responseText;
                    var errorMsg = text;
                    if (_this.ajax.status == 200) {
                        try {
                            errorMsg = "";
                            var jdata = JSON.parse(text);
                            cb(jdata);
                        } catch (e) {
                            errorMsg = "调用成功回调出错：" + e;
                        }
                    }

                    if (errorMsg) {
                        if (error) {
                            error(errorMsg);
                        } else {
                            alert(errorMsg);
                        }
                    }
                }
            }
        } catch (e) {
            alert(e);
        }
    }
}