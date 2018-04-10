/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.LogisticsSendBillAction = function() {  
    Test001.LogisticsSendBillAction.initializeBase(this);  
}

Test001.LogisticsSendBillAction.prototype = {  
    initialize: function() {  
        Test001.LogisticsSendBillAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.LogisticsSendBillAction.callBaseMethod(this, 'dispose');  
    },

    doSure: function (sender, e) {
        var form = sender.get_form();
        if (confirm("确认已签收？")) {
            var id = e._item.bid.get_text();
            form.get_service().Sure(id, function () {
                e._item.dataImage1.set_visible(false)
            });
        }
    }
}
Test001.LogisticsSendBillAction.registerClass('Test001.LogisticsSendBillAction', Sys.UI.PageAction);
