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

    loadForm: function (form, tag) {
        this.showPageTag = tag;
        if (tag == "0" || tag == "2") {
            this.childForm.refresh("Wait.gspx");
            return;
        }
        var content = form.container.get_element();
        this.childForm = new Sys.UI.CustomForm(this);
        // 中间栏目的子窗体，用于显示其它控件页面
        this.childForm.set_clientSize("ClientHeight=" + (content.offsetHeight) + ",ClientWidth=" + (content.offsetWidth - 30));
        this.childForm._contentElement = content;
        this.childForm.add_loaded(this.doMiddleFormLoaded, this);
        var url = tag == "1" ? "~/BillList.gspx" : "~/StatisticsPage.gspx";
        url = url + "?area=" + form.area.get_value();
        this.childForm.refresh(url);
    },

    doMiddleFormLoaded: function (frm) {
        frm._initialized = true;
    },

    chatItemClick: function (sender) {
        var buttonArray = ["tj", "dd","jqzc"];
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
                this.childForm.refresh("~/StatisticsPage.gspx?area=" + sender.get_value());
                break;
            case "1":
                this.childForm.refresh("~/BillList.gspx?area=" + sender.get_value());
                break;
        }
    },

    onReload: function (sender) {
        var form = sender.get_form();
        form.refresh();
    },

    doRefresh: function (sender) {
        var form = sender.get_form();
        this.loadForm(form, this.showPageTag);
    }
}
WebMain.DefaultAction.registerClass('WebMain.DefaultAction', Sys.UI.PageAction);