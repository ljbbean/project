/// <reference path="docs/carpa-vsdoc.js" />
 
Type.registerNamespace('WebMain');

WebMain.DefaultAction = function () {
    WebMain.DefaultAction.initializeBase(this);
    this.showPageTag = "1";
}

WebMain.DefaultAction.prototype = {
    initialize: function () {
        WebMain.DefaultAction.callBaseMethod(this, 'initialize');
        var form = this.get_form();
        var userData = form.user.get_value();
        if (userData && userData.length > 0) {
            form.reload.set_visible(false);
            this.loadForm(form, this.showPageTag);
        } else {
            form.reload.set_visible(true);
        }
    },

    loadForm: function (form, tag, re) {
        this.showPageTag = tag;
        if (tag == "0") {
            form.mdi.showPage("Wait.gspx");
            return;
        }
        var url; ;
        switch (tag) {
            case "0":
                url = "~/StatisticsPage.gspx";
                break;
            case "1":
                url = "~/BillList.gspx";
                break;
            case "2":
                url = "~/DataHandler/MilitaryInvestigation.gspx";
                break;
        }
        if (re) {
            url = url + "?area=" + form.area.get_value() + "&re=" + re;
        } else {
            url = url + "?area=" + form.area.get_value();
        }
        form.mdi.showPage(url);
    },

    chatItemClick: function (sender) {
        var buttonArray = ["tj", "dd", "jqzc"];
        var form = sender.get_form();
        for (var i in buttonArray) {
            form[buttonArray[i]].get_element().className = "buttonNoSelected";
        }
        var tag = sender.get_tag();
        sender.get_element().className = "button";
        this.loadForm(form, sender.get_tag());
    },

    doAreaChange: function (sender) {
        var form = sender.get_form();
        switch (this.showPageTag) {
            case "0":
                form.mdi.showPage("~/StatisticsPage.gspx?area=" + sender.get_value());
                break;
            case "1":
                form.mdi.showPage("~/BillList.gspx?area=" + sender.get_value());
                break;
        }
    },

    onReload: function (sender) {
        var form = sender.get_form();
        form.refresh();
    },

    doRefresh: function (sender) {
        var form = sender.get_form();
        this.loadForm(form, this.showPageTag, new Date());
    }
}
WebMain.DefaultAction.registerClass('WebMain.DefaultAction', Sys.UI.PageAction);