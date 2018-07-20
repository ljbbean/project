/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain.DataHandler');

WebMain.DataHandler.MilitaryInvestigationAction = function () {
    WebMain.DataHandler.MilitaryInvestigationAction.initializeBase(this);
    this.detailArray = {};
    this.socket = null;
}

WebMain.DataHandler.MilitaryInvestigationAction.prototype = {
    initialize: function () {
        WebMain.DataHandler.MilitaryInvestigationAction.callBaseMethod(this, 'initialize');
        this.doDataFormLoaded(this.get_form());
    },

    dispose: function () {
        WebMain.DataHandler.MilitaryInvestigationAction.callBaseMethod(this, 'dispose');
    },

    doSearch: function (sender) {
        var form = sender.get_form();

        var condition = form.condition.get_value();
        if (!condition || condition.length == 0) {
            alert('请录入侦查条件');
            return;
        }
        var data = JsObj.getMilitary(condition);

        var tempData = JSON.parse(data);
        for (var i in tempData) {
            var item = tempData[i];
            item.pic_url = "https:" + tempData[i].pic_url;
            item.buttons = "<font color='#999999' style='cursor:pointer;' title='查看详情'>查看详情</font>";
        }
        form.grid.dataBind(tempData);
    },

    doDataFormLoaded: function (sender) {
        var form = sender;
        var socket = this._connectWebsocket(sender);
        var t = new Date().getTime();
        this.uid = form.user.get_value() + "_" + t;
        socket.emit("login", { uid: this.uid })
    },

    _connectWebsocket: function (form) {
        var url = form.socketurl.get_value();
        if (url.length == 0) {
            alert('未设置socket对应的地址');
            return null;
        }
        if (this.socket) {
            return this.socket;
        }
        var _this = this;
        this.socket = io.connect(url);
        this.socket.on("goodMsg", function (data) {
            var items = {};
            try {
                items = JSON.parse(data.msg);
            } catch (e) {
                alert(data.msg);
                return;
            }

            if (items.gid) {//单商品
                var id = items.gid.toString();
                var data = form.grid.get_dataSource();
                var index = form.grid.get_selectedRowIndex();
                for (var i in data) {
                    var row = data[i];
                    if (!row.id) {
                        row.id = _this.getGoodId(row.detail_url);
                    }
                    if (row.id.toString() == id) {
                        row.start = items.startDate;
                        row.end = items.endDate;
                        var cache = _this.detailArray[row.detail_url];
                        if (cache) {
                            cache.Start = row.start;
                            cache.End = row.end;
                        }
                    }
                    if (i == index) {
                        _this.doGridChange(_this);
                    }
                }
                return;
            }
            for (var i in items) {
                var item = items[i];
                item.raw_title = item.title;
                item.start = item.startDate;
                item.end = item.endDate;
                item.create = item.create;
                item.detail_url = item.pic_url = "https:" + item.url;
                item.buttons = "<font color='#999999' style='cursor:pointer;' title='查看详情'>查看详情</font>";
            }
            form.grid.dataBind(items);
        });

        return this.socket;
    },

    doRefresh: function (sender) {
        var form = sender.get_form();
        form.refresh();
    },

    doGridChange: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data || !data.detail_url) {
            return;
        }
        var detail_url = data.detail_url;
        var detail = this.detailArray[detail_url];
        if (!detail) {
            detail = JsObj.getDetail(data.detail_url);
            this.getGoodMsg(form, detail_url);
            detail = JSON.parse(detail);
            this.detailArray[detail_url] = detail;
        }

        if (detail.error) {
            delete this.detailArray[detail_url];
            console.log(detail.error);
            return;
        }
        form.gridSku.dataBind(detail.List);
        form.fhd.set_text(detail.SendCity);
        form.qrl.set_text(detail.ConfirmGoodsCount);
        form.xsl.set_text(detail.SoldTotalCount);
        form.zffs.set_text(detail.Pays);
        form.fw.set_text(detail.Service);
        form.yhj.set_text(detail.Coupon);
        form.start.set_text(detail.Start);
        form.end.set_text(detail.End);
    },

    doButtonClick: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data || !data.detail_url) {
            return;
        }
        var detail_url = data.detail_url;
        window.open(detail_url, "_blank")
    },

    doGet: function (sender) {
        var form = sender.get_form();
        var value = form.saveData();
        this.getGoodMsg(form, value.condition);
    },

    getGoodMsg: function (form, condition) {
        var id = this.getGoodId(condition);
        if (id.length > 0) {
            form.get_service().GetSingleGoodMsg(id, this.uid, function () {

            });
        } else {
            form.get_service().GetGoodMsg(condition, this.uid, function () {
            });
        }
    },

    getGoodId: function (condition) {
        var items = /id=[0-9]+/.exec(condition);
        return items && items.length == 1 ? items[0].substr(3) : "";
    }
}
WebMain.DataHandler.MilitaryInvestigationAction.registerClass('WebMain.DataHandler.MilitaryInvestigationAction', Sys.UI.PageAction);
