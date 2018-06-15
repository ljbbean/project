/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.DataHandler'); 

Test001.DataHandler.DataCatchFromTBExampleAction = function() {  
    Test001.DataHandler.DataCatchFromTBExampleAction.initializeBase(this);  
}

Test001.DataHandler.DataCatchFromTBExampleAction.prototype = {
    initialize: function () {
        Test001.DataHandler.DataCatchFromTBExampleAction.callBaseMethod(this, 'initialize');
        this.socket = null;
        this.doDataFormLoaded(this.get_form());
    },

    dispose: function () {
        Test001.DataHandler.DataCatchFromTBExampleAction.callBaseMethod(this, 'dispose');
    },

    gridDataBind: function (grid, data) {
        var source = grid.get_dataSource()
        if (!source) {
            source = [];
        }
        if (data.url || data.msg.indexOf("OK:") == 0 || data.msg.indexOf("Exception:") == 0) {
            for (var i in source) {
                source[i].url = "";//���֮ǰ������
            }
        }
        var fuid = data.fuid;
        if (fuid.indexOf('_') >= 0) {
            data.fuid = fuid.substring(0, fuid.indexOf('_'));
        }
        Array.insert(source, 0, data);
        grid.dataBind(source);
    },

    _getTempData: function (form, id, callBack) {
        form.get_service().GetTempData(id, function (tData) {

            var bill = tData.bill;
            for (var i = 0; i < bill.length; i++) {
                var item = bill[i];
                if (item.senddate == "null") {
                    item.senddate = "";
                }
                if (item.successdate == "null") {
                    item.successdate = "";
                }
                if (item.date == "null") {
                    item.date = "";
                }
                bill[i] = item;
            }
            tData.bill = bill;
            callBack(tData);
            var billForm = new Sys.UI.Form(form);
            billForm.data = tData;
            billForm.add_loaded(function () {
                billForm.grid.dataBind(tData.bill);
            })
            billForm.showModal("BillList.gspx");
            return;
        })
    },

    _connectWebsocket: function (form) {
        var url = form.socketurl.get_value();
        var grid = form.grid;
        if (url.length == 0) {
            alert('δ����socket��Ӧ�ĵ�ַ');
            return null;
        }
        if (this.socket) {
            return this.socket;
        }
        var _this = this;
        this.socket = io.connect(url);
        this.socket.on("tb_qr_url", function (data) {
        });
        this.socket.on("receiveMsg", function (data) {
            var key = "OK:url:";
            if (data.msg.indexOf(key) == 0) {
                //ֱ��Ԥ��
                var id = data.msg.substring(key.length);
                _this._getTempData(form, id, function (ndata) {
                    data.msg = "�����������أ�����ͨ��˫���������ݲ鿴������";
                    data.data = ndata;
                    _this.gridDataBind(grid, data);
                });
            } else {
                _this.gridDataBind(grid, data);
            }
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
            var url = "";
            switch (data.type) {
                case "exit":
                    msg = '�˳�����';
                    break;
                case "loginSuccess":
                    msg = '��¼�ɹ�';
                    break;
                case "reLogin":
                    msg = '���µ�¼�ɹ�';
                    break;
                case "refuse":
                    msg = '�ܾ��������ط���¼';
                    break;
                case "failed":
                    msg = data.msg && data.msg.length != 0 ? data.msg:'����������';
                    break;
                case "doing":
                    msg = data.msg;
                    url = data.url;

                    break;
                default:
                    msg = data.type + '  Ϊ�Ƿ�����';
            }

            _this.gridDataBind(grid, {
                date: data.date,
                msg: msg,
                url: url,
                fuid: '����������'
            })
        })
        return this.socket;
    },

    doDataFormLoaded: function (sender) {
        var form = sender;
        var socket = this._connectWebsocket(sender);
        var uid = form.uid.get_value();
        if (socket == null || !uid) {
            $debug("��ǰ�û���"+ uid);
            return;
        }
        socket.emit("login", { uid: uid })
    },

    doGetQR: function (sender) {
        var form = sender.get_form();
        var uid = form.uid.get_value();
        if (this.socket == null) {
            alert('��δ��¼����ˢ�±�ҳ�����Ե�¼');
            return;
        }

        this.socket.emit("getTB_QR", {uid:uid})
    },

    doGridDbClick: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data) {
            return;
        }
        var form1 = new Sys.UI.Form(sender);
        form1.add_loaded(function () {
            var ndata = data.data;
            form1.data = ndata;
            form1.grid.dataBind(ndata.bill);
        });
        form1.showModal("BillList.gspx");
    }
}
Test001.DataHandler.DataCatchFromTBExampleAction.registerClass('Test001.DataHandler.DataCatchFromTBExampleAction', Sys.UI.PageAction);
