/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.LoginAction = function() {  
    Test001.LoginAction.initializeBase(this);  
    this.socket = null;
}

Test001.LoginAction.prototype = {
    initialize: function () {
        Test001.LoginAction.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        Test001.LoginAction.callBaseMethod(this, 'dispose');
    },

    doLogin: function (sender) {
        var form = sender.get_form();
        form.get_service().UserLogin(window.screen.availWidth, window.screen.availHeight, form.name.get_value(), form.pwd.get_value(), function () {
            if (form.isChecked.get_checked()) {
                window.location = "SendSimpleTemplateBill.gspx";
            } else {
                window.location = "BillList.gspx";
            }
        });
    }
}
Test001.LoginAction.registerClass('Test001.LoginAction', Sys.UI.PageAction);
