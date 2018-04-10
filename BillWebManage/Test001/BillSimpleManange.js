/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.BillSimpleManangeAction = function() {  
    Test001.BillSimpleManangeAction.initializeBase(this);  
    this.sizecolor = null;
}

Test001.BillSimpleManangeAction.prototype = {  
    initialize: function() {  
        Test001.BillSimpleManangeAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function () {
        this.sizecolor = null;
        Test001.BillSimpleManangeAction.callBaseMethod(this, 'dispose');  
    },

    doSave: function(sender) {
        var form = sender.get_form();
        var data = form.saveData();
        form.get_service().DoSave(data, function () {
            form._data = data;
            form.doOk();
        });
    },

    doTotalChange: function(sender) {
        var form = sender.get_form();
        var total = this.getValue(form.total);
        var btotal = this.getValue(form.btotal);
        var preferential = this.getValue(form.preferential);
        form.ltotal.set_value(total - btotal - preferential);
    },

    getValue: function (temp) {
        var value = temp.get_value();
        if (value) {
            return value;
        }
        return 0;
    },

    doTotalPriceChange: function (sender) {
        var form = sender.get_form();
        var total = this.getValue(form.total);
        var preferential = this.getValue(form.preferential);
        
        form.btotal.set_value((total - preferential) * (form.dobtotal.get_value() == "1" ? 0 : 0.05));
        this.doTotalChange(sender);
    },

    doAddressBlur: function (sender) {
        var form = sender.get_form();
        var value = form.caddress.get_value();
        var ctel = form.ctel.get_value();
        var cname = form.cname.get_value();
        var carea = form.carea.get_value();
        if (!value) {
            return;
        }
        var varray = value.split(',');
        if (varray.length < 3) {
            return;
        }
        if (!ctel || ctel.length == 0) {
            form.ctel.set_value(varray[1].trim());
        }
        if (!cname || cname.length == 0) {
            form.cname.set_value(varray[0].trim());
        }
        if (!carea || carea.length == 0) {
            var temp = varray[2].trim().split(' ');
            form.carea.set_value(temp.length > 2 ? (temp[0] + ',' + temp[1] + ',' + temp[2]) : (temp.length > 1 ? (temp[0] + ',' + temp[1]):temp));
        }
    },

    SizeChange: function (sender) {
        this.sizeColorChange(sender);
    },

    ColorChange: function (sender) {
        this.sizeColorChange(sender);
    },

    sizeColorChange: function (sender) {
        var form = sender.get_form();
        var value = form.sizecolor.get_value();
        var size = form.size.get_value();
        var color = form.color.get_value();
        for (var i = 0, count = value.length; i < count; i++) {
            if (value[i]["color"] == color && value[i]["size"] == size) {
                form.total.set_value(value[i]["price"]);
                break;
            }
        }
        this.doTotalPriceChange(sender);
    }
}
Test001.BillSimpleManangeAction.registerClass('Test001.BillSimpleManangeAction', Sys.UI.PageAction);
