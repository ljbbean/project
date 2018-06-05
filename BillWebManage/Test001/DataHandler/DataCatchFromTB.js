/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.DataHandler'); 

Test001.DataHandler.DataCatchFromTBAction = function() {  
    Test001.DataHandler.DataCatchFromTBAction.initializeBase(this);  
}

Test001.DataHandler.DataCatchFromTBAction.prototype = {
    initialize: function () {
        Test001.DataHandler.DataCatchFromTBAction.callBaseMethod(this, 'initialize');
        this.socket = null;
    },

    dispose: function () {
        Test001.DataHandler.DataCatchFromTBAction.callBaseMethod(this, 'dispose');
    },

    gridDataBind: function (grid, data) {
        var source = grid.get_dataSource()
        if (!source) {
            source = [];
        }
        Array.insert(source, 0, data);
        $debug.traceDump(data)
        grid.dataBind(source);
    },

    _connectWebsocket: function (form) {
        var url = form.socketurl.get_value();
        var grid = form.grid;
        if (url.length == 0) {
            alert('未设置socket对应的地址');
            return null;
        }
        if (this.socket) {
            return this.socket;
        }
        var _this = this;
        this.socket = io.connect(url);
        this.socket.on("tb_qr_url", function (data) {
            form.qr.set_src(data.url);
            form.qr.set_visible(true);
        });
        this.socket.on("receiveMsg", function (data) {
            _this.gridDataBind(grid, data);
        });
        this.socket.on("ask", function (data) {
            if (confirm(data.msg)) {
                _this.socket.emit("sure", { id: data.id, msg: "ok" });
            } else {
                _this.socket.emit("sure", { id: data.id, msg: "no" });
            }
        })

        this.socket.on("exec", function (data) {
            var msg = "";
            switch (data.type) {
                case "exit":
                    msg = '退出连接';
                    break;
                case "loginSuccess":
                    msg = '登录成功';
                    break;
                case "reLogin":
                    msg = '重新登录成功';
                    break;
                case "refuse":
                    msg = '拒绝在其他地方登录';
                    break;
                case "failed":
                    msg = '服务器推送';
                    break;
                case "doing":
                    msg = data.msg;
                    break;
                default:
                    msg = data.type + '  为非法命令';
            }

            _this.gridDataBind(grid, {
                date: data.date,
                msg: msg,
                fuid: '服务器推送'
            })
        })
        return this.socket;
    },

    doDataFormLoaded: function (sender) {
        var form = sender;
        var socket = this._connectWebsocket(sender);
        var uid = form.uid.get_value();
        if (socket == null || !uid) {
            $debug("当前用户："+ uid);
            return;
        }

        socket.emit("login", { uid: uid })
    },

    doGetQR: function (sender) {
        var form = sender.get_form();
        var uid = form.uid.get_value();
        if (this.socket == null) {
            alert('还未登录，请关闭当前页面后重新进入');
            return;
        }

        this.socket.emit("getTB_QR", {uid:uid})
    }
}
Test001.DataHandler.DataCatchFromTBAction.registerClass('Test001.DataHandler.DataCatchFromTBAction', Sys.UI.PageAction);
