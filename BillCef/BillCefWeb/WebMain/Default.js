/// <reference path="docs/carpa-vsdoc.js" />
 
Type.registerNamespace('WebMain');

WebMain.DefaultAction = function() {
    WebMain.DefaultAction.initializeBase(this);
}

WebMain.DefaultAction.prototype = {
    initialize: function () {
        WebMain.DefaultAction.callBaseMethod(this, 'initialize');
        var content = this.get_form().container.get_element();
        this.childForm = new Sys.UI.CustomForm(this);
        // 中间栏目的子窗体，用于显示其它控件页面
        this.childForm.set_clientSize("ClientHeight=" + (content.offsetHeight) + ",ClientWidth=" + (content.offsetWidth - 30));
        this.childForm._contentElement = content;
        this.childForm.add_loaded(this.doMiddleFormLoaded, this);
        this.childForm.refresh("~/StatisticsPage.gspx?area=0");
    },
    
    doMiddleFormLoaded: function (frm) {
        frm._initialized = true;
    },

    chatItemClick: function (sender) {
        var buttonArray = ["tj", "dd"];
        var form = sender.get_form();
        for (var i in buttonArray) {
            form[buttonArray[i]].get_element().className = "buttonNoSelected";
        }
        sender.get_element().className = "button";
    },

    doAreaChange: function (sender) {
        var form = sender.get_form();
        this.childForm.refresh("~/StatisticsPage.gspx?area=" + sender.get_value());
    }
}
WebMain.DefaultAction.registerClass('WebMain.DefaultAction', Sys.UI.PageAction);