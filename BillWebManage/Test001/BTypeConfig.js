/// <reference path="docs/carpa-vsdoc.js" />
 
Type.registerNamespace('Test001');

Test001.BTypeConfigAction = function() {
    Test001.BTypeConfigAction.initializeBase(this);
}

Test001.BTypeConfigAction.prototype = {
    initialize: function() {
        Test001.BTypeConfigAction.callBaseMethod(this, 'initialize');
    },
    
    dispose: function() {
        Test001.BTypeConfigAction.callBaseMethod(this, 'dispose');
    },
    
    doSave: function(sender) {
        var form = sender.get_form();
        var data = form.grid.saveData();
        form.get_service().DoSave(data, function () {
            form.doOk();
        });
    },

    doBeginEdit: function (sender, args) {
        var node = args.get_node();
        args.set_cancel(true);   
        if (node.get_level() == 0 && (node.get_text() == "尺码" || node.get_text() == "颜色")) {
            sender.set_addFlag(2);      
            args.set_allowDeleteChild(false);                
            return;
        }
        
        if (node.get_level() >= 1) {
            sender.set_addFlag(1);        
        }
    },

    doNodeAdd: function (sender, args) {
        var form = sender.get_form();
        var grid = form.grid;
        var data = {
            size: "",
            color: "",
            price:0
        }
        var parentNode = args.get_parentNode();
        var childNodes = parentNode.get_children();
        var nodeText = args.get_text().trim();
        for (var i = 0, count = childNodes.length; i < count; i++) {
            if (childNodes[i].get_text().trim() == nodeText) {
                args.set_cancel(true);
                alert('同级不能重名');
                return;
            }
        }
        if (args.get_parentNode().get_text() == "尺码") {
            var colorNode = sender.locateNode("name", "颜色");
            var colorChildNodes = colorNode.get_children();
            for (var i = 0, count = colorChildNodes.length; i < count; i++) {
                grid.appendRowData({
                    size: nodeText,
                    color: colorChildNodes[i].get_text().trim(),
                    price: 0
                })
            }
        } else {
            var sizeNode = sender.locateNode("name", "尺码");
            var sizeChildNodes = sizeNode.get_children();
            for (var i = 0, count = sizeChildNodes.length; i < count; i++) {
                grid.appendRowData({
                    size: sizeChildNodes[i].get_text().trim(),
                    color: nodeText,
                    price: 0
                })
            }
        }
    },

    doNodeDelete: function (sender, args) {
        var node = args.get_node();
        var parentNode = node.get_parent();
        var form = sender.get_form();
        var grid = form.grid;
        var data = grid.saveData();
        var dataField = parentNode.get_text() == "颜色"?"color":"size";
        for (var i = data.length - 1; i >= 0; i--) {
            if (data[i][dataField] == node.get_text()) {
                Array.removeAt(data, i);
            }
        }
        grid.dataBind(data);
    }
}
Test001.BTypeConfigAction.registerClass('Test001.BTypeConfigAction', Sys.UI.PageAction);