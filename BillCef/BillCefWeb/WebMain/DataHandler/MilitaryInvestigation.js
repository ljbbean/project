/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain.DataHandler');

WebMain.DataHandler.MilitaryInvestigationAction = function () {
    WebMain.DataHandler.MilitaryInvestigationAction.initializeBase(this);
    this.detailArray = {};
}

WebMain.DataHandler.MilitaryInvestigationAction.prototype = {
    initialize: function () {
        WebMain.DataHandler.MilitaryInvestigationAction.callBaseMethod(this, 'initialize');
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
        var detailContent = this.detailArray[detail_url];
        if (!detailContent) {
            detailContent = JsObj.getDetail(data.detail_url);
            this.detailArray[detail_url] = detailContent;
        }

        var detail = JSON.parse(detailContent);
        if (detail.error) {
            delete this.detailArray[detail_url];
            alert(detail.error);
            return;
        }
        form.gridSku.dataBind(detail.List);
        form.fhd.set_text(detail.SendCity);
        form.qrl.set_text(detail.ConfirmGoodsCount);
        form.xsl.set_text(detail.SoldTotalCount);
        form.zffs.set_text(detail.Pays);
        form.fw.set_text(detail.Service);
        form.yhj.set_text(detail.Coupon);
    },

    doButtonClick: function (sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data || !data.detail_url) {
            return;
        }
        var detail_url = data.detail_url;
        window.open(detail_url, "_blank")
    }
}
WebMain.DataHandler.MilitaryInvestigationAction.registerClass('WebMain.DataHandler.MilitaryInvestigationAction', Sys.UI.PageAction);
