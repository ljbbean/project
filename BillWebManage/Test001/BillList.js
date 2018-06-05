/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.BillListAction = function() {  
    Test001.BillListAction.initializeBase(this);  
}

Test001.BillListAction.prototype = {
    initialize: function () {
        Test001.BillListAction.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        Test001.BillListAction.callBaseMethod(this, 'dispose');
    },

    doAdd: function (sender) {
        var form = new Sys.UI.Form(sender);

        form.add_closed(function () {
            var data = form.saveData();
            sender.get_form().grid.get_pager().refresh(data);
        });
        form.showModal("BillManage.gspx");
    },

    doGridSelectedChanged: function(sender) {
        var form = sender.get_form();
        var rowData = form.grid.get_selectedRowData();
        if (!rowData) {
            form.gridDetail.dataBind(null);
            return;
        }
        form.get_service().GetDetail(rowData.id, function (data) {
            form.gridDetail.dataBind(data);
        });
    },

    doSearch: function(sender) {
        var form = sender.get_form();
        var data = form.saveData();
        form.grid.get_pager().refresh(data);
        form.gridDetail.dataBind(null);
    },

    doSimpleAdd: function (sender) {
        var form = new Sys.UI.Form(sender);

        form.add_closed(function () {
            var data = form.saveData();
            sender.get_form().grid.get_pager().refresh(data);
        });
        form.showModal("BillSimpleManange.gspx");
    },

    doGridDeleting: function(sender, e) {
        if (!confirm('确认要删除该条数据？')) {
            e.set_cancel(false);
            return;
        }
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if(!data){
            return;
        }
        form.get_service().DeleteBill(data.id, function (msg) {
            alert(msg);
        });
    },

    doGridDbClick: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data) {
            return;
        }
        var form1 = new Sys.UI.Form(sender);
        form1.add_closed(function () {
            var data = form1._data;
            if (data) {
                sender.get_form().grid.modifySelectedRowData(data);
            }
        });
        form1.showModal("BillSimpleManange.gspx?id=" + data.id);
    },

    doSendClick: function (sender, eventArgs) {
        var form = sender.get_form();
        if (eventArgs._buttonText == "待发货") {
            form.pmMain.popup(eventArgs.get_event());
        }
        if (eventArgs._buttonText == "已发货") {
            form.sendedMain.popup(eventArgs.get_event());
        }
        if (eventArgs._buttonText == "已签收") {
            form.sureMain.popup(eventArgs.get_event());
        }
    },

    doSelfSend: function (sender) {
        this._doStatus(sender);
    },

    doOtherSend: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data) {
            return;
        }
        var tform = new Sys.UI.Form(sender);
        var _this = this;
        tform.add_loaded(function () {
            tform.id = data.id;
        });

        tform.add_closed(function () {
            if (!tform._ok) {
                return;
            }
            _this._doChangeStatus(data, form, 1);
        });
        tform.showModal("SendGoods.gspx");
    },

    doCustomSend: function(sender) {
        this.doSelfSend(sender);
    },

    doRowRending: function (sender, args) {
        var rowIndex = args.get_rowIndex();
        var rowData = sender.findRowData(rowIndex);
        if (!rowData) return;
        var color = "";
        switch (rowData.status) {
            case 0:
                color = "#000000";
                args.set_bgColor("#E35850");
                break;
            case 1:
                color = "#000000";
                args.set_bgColor("#80bbfa");
                break;
            case 2:
                color = "#F213C8";
                break;
            case 3:
                color = "#5A3222";
                break;
            case 4:
                color = "red";
                break;
        }
        args.set_fontColor(color);
    },

    doSended: function (sender) {
        this._doStatus(sender);
    },

    doSureSended: function(sender) {
        this._doStatus(sender);
    },

    _doStatus: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data) {
            return;
        }
        var _this = this;
        form.get_service().doStatus(data.id, data.status, function (rt) {
            _this._doChangeStatus(data, form, rt);
        });
    },

    _doChangeStatus : function(data, form, rt){
        if (rt == 1) {
            data.status = data.status + 1;
            switch (data.status) {
                case 1:
                    data.process = '已发货';
                    break;
                case 2:
                    data.process = '已签收';
                    break;
                case 3:
                    data.process = '已确认';
                    break;
            }
            form.grid.modifySelectedRowData(data);
        } else {
            alert('修改状态错误，请重试！');
        }
    },

    doSendMsgClick: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data) {
            return;
        }
        var tform = new Sys.UI.Form();
        tform.add_loaded(function () {
            switch (data.status) {
                case 0:
                case 1:
                case 2:
                    tform.sure.set_visible(true);
                    break;
            }
        });
        tform.add_closed(function () {
            if (tform.isSure) {
                data.status = 3
                data.process = "已确认";
                form.grid.modifySelectedRowData(data);
            }
        });
        tform.tid = data.id;
        var prefix = '安能';
        tform.showModal("Logistics/LogisticsInformation.gspx?code=" + data.scode + "&r=" + Math.random() + "&n=" + (data.sname.slice(0, prefix.length) === prefix ? 0:1));
        //window.open("http://www.kuaidi100.com/query?type=kuaijiesudi&postid=" + data.scode + "&id=1&valicode=&temp=" + Math.random());
    },

    doAfterClick: function (sender, eventArgs) {
        var form = sender.get_form();
        if (eventArgs._buttonText == "运险保证") {
            form.sendAfterMain.popup(eventArgs.get_event());
        }
        if (eventArgs._buttonText == "签售保证") {
            form.getAfterMain.popup(eventArgs.get_event());
        }
        if (eventArgs._buttonText == "售后保证") {
            form.useAfterMain.popup(eventArgs.get_event());
        }
    },

    doAfter: function(sender, status) {
        var form = sender.get_form();
        var id = sender.get_id();
        var tform = new Sys.UI.Form(sender);
        var data = form.grid.get_selectedRowData();
        tform.add_loaded(function () {
            tform.set_caption("发货处理(" + sender.get_text() + ")");
            tform.bid.set_value(data.id);
            tform.status.set_value(status);
        });
        tform.showModal("AffiliateBill.gspx");
    },

    doAfter0: function(sender) {
        this.doAfter(sender, 0);
    },

    doAfter1: function (sender) {
        this.doAfter(sender, 1);
    },

    doAfter2: function (sender) {
        this.doAfter(sender, 2);
    },

    doAfter3: function (sender) {
        this.doAfter(sender, 3);
    },

    doAfter4: function (sender) {
        this.doAfter(sender, 4);
    },

    raiseExpandChange: function (sender, eventArgs) {
        if (!eventArgs.get_value()) return;
        var form = this.get_form();

        var rowData = this.get_form().grid.getRowData(eventArgs.get_rowIndex());
        eventArgs.set_pageUrl("AffiliateBillList.gspx?id=" + rowData.id);
    },

    doStock: function(sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("StockBill.gspx");
    },

    doAfter5: function (sender) {
        this.doAfter(sender, 5);
    },

    doSendSimple: function(sender) {
        window.location = "SendSimpleTemplateBill.gspx";
    },

    doBackSection: function (sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("FundsManager/BackSection.gspx");
    },
    doBtypeConfig: function (sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("BTypeConfig.gspx");
    },

    doBackSectionDetail: function (sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("FundsManager/BackSectionDetailList.gspx");
    },

    excelSure: function (sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("ExcelSure.gspx");
    },

    doDataCatch: function (sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("DataCatch.gspx");
    },

    doCatchFromTB: function (sender) {
        var form = new Sys.UI.Form(sender);
        form.showModal("DataHandler/DataCatchFromTB.gspx");
    }
}
Test001.BillListAction.registerClass('Test001.BillListAction', Sys.UI.PageAction);
