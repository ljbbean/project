/// <reference path="docs/carpa-vsdoc.js" />
 
Type.registerNamespace('Test001');

Test001.ExcelSureAction = function() {
    Test001.ExcelSureAction.initializeBase(this);
}

Test001.ExcelSureAction.prototype = {
    initialize: function() {
        Test001.ExcelSureAction.callBaseMethod(this, 'initialize');
    },
    
    dispose: function() {
        Test001.ExcelSureAction.callBaseMethod(this, 'dispose');
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
        if (node.get_level() == 0 && (node.get_text() == "����" || node.get_text() == "��ɫ")) {
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
                alert('ͬ����������');
                return;
            }
        }
        if (args.get_parentNode().get_text() == "����") {
            var colorNode = sender.locateNode("name", "��ɫ");
            var colorChildNodes = colorNode.get_children();
            for (var i = 0, count = colorChildNodes.length; i < count; i++) {
                grid.appendRowData({
                    size: nodeText,
                    color: colorChildNodes[i].get_text().trim(),
                    price: 0
                })
            }
        } else {
            var sizeNode = sender.locateNode("name", "����");
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
        var dataField = parentNode.get_text() == "��ɫ"?"color":"size";
        for (var i = data.length - 1; i >= 0; i--) {
            if (data[i][dataField] == node.get_text()) {
                Array.removeAt(data, i);
            }
        }
        grid.dataBind(data);
    },

    doFileUpload: function FileUploadAction$doFileUpload(sender) {
        Sys.UI.Controls.FileUpload.submit(sender, Function.createDelegate(this, this.doFileUploadSucceeded), Function.createDelegate(this, this.doFaild));
    },

    doFaild: function (sender, message) {
        $debug.traceDump(message);
    },

    doFileUploadSucceeded: function FileUploadAction$doFileUploadSucceeded(sender, result) {
        var grid = this.get_form().grid;
        var columns = grid.get_columns();
        for (var i = columns.length - 1; i >= 0; i--) {
            grid.removeColumn(i);
        }
        this._index = 0;
        for (var i = 0; i < result.ColumnDataFields.length; i++) {
            this.createTextColumn(result.ColumnDataFields[i]);
        }

        grid.dataBind(result.Data);
    },

    createTextColumn: function (caption) {
        var params = {
            type: Sys.UI.Controls.TextColumn,
            caption: caption,
            dataField: caption,
            width:160
            //,events: {
            //    dblClick: Function.createDelegate(this, this.doDblClick),
            //    change: Function.createDelegate(this, this.doChange)
            //}
        };
        //if (caption == "״̬") {
        //    params = {
        //        type: Sys.UI.Controls.DropDownColumn,
        //        caption: caption,
        //        dataField: caption,
        //        properties: {
        //            items: [{ value: '', text: "ȫ��" },
        //                {
        //                    value: '�ظ�', text: "�ظ�"
        //                },
        //                {
        //                    value: 'δƥ��', text: "δƥ��"
        //                },
        //                {
        //                    value: '��ƥ��', text: "��ƥ��"
        //                }],
        //            dropDownStyle: Sys.UI.Controls.DropDownStyle.dropDownSearch,
        //            filterType: Sys.UI.Controls.DataFilterType.range
        //        }
        //    };
        //}
        this.createColumnByParams(params);
    },

    createColumnByParams: function (params) {
        var grid = this.get_form().grid;
        var column = null;
        if (this._index == -1) {
            column = grid.appendColumn(params);
        } else {
            column = grid.insertColumn(this._index, params); // ָ�����������У��ڲ����ж������Ƿ�Ϸ������Ϸ���Ϊ׷���У�
        }
        return column;
    },

    doStateChange: function (sender) {
        var form = sender.get_form();
        var value = form.state.get_value();
        var grid = form.grid;
        grid.filter(value == "0" ? "" : value);
    }
}
Test001.ExcelSureAction.registerClass('Test001.ExcelSureAction', Sys.UI.PageAction);