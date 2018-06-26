/** 
 * @fileoverview Carpa3 JavaScript文档
 *
 * @author 任我行 TRD<br>
 * - 2010-03-15 重新生成
 * - 2009-03-23 创建文件
 */

Array.add = function(array, item) {
/// <summary>添加一个项</summary>
/// <param name="array" type="Array"></param>
/// <param name="item"></param>
}

Array.addRange = function(array, items) {
/// <summary>添加另一个数组中的项</summary>
/// <param name="array" type="Array"></param>
/// <param name="items" type="Array"></param>
}

Array.clear = function(array) {
/// <summary>清除所有项</summary>
/// <param name="array" type="Array"></param>
}

Array.clone = function(array) {
/// <summary>浅克隆一个数组</summary>
/// <param name="array" type="Array"></param>
/// <returns type="Array elementMayBeNull="true""></returns>
}

Array.contains = function(array, item) {
/// <summary>返回数组是否包含某项</summary>
/// <param name="array" type="Array"></param>
/// <param name="item"></param>
/// <returns type="Boolean"></returns>
}

Array.enqueue = function(array, item) {
/// <summary>添加一个项</summary>
/// <param name="array" type="Array"></param>
/// <param name="item"></param>
}

Array.getFirst = function(array) {
/// <summary>取第一个元素，如果没有则返回 undefined</summary>
/// <param name="array"></param>
}

Array.getLast = function(array) {
/// <summary>取最后一个元素，如果没有则返回 undefined</summary>
/// <param name="array"></param>
}

Array.indexOf = function(array, item, start) {
/// <summary>返回数组包含某项的索引，找不到则返回 -1</summary>
/// <param name="array" type="Array"></param>
/// <param name="item"></param>
/// <param name="start"></param>
/// <returns type="Number"></returns>
}

Array.insert = function(array, index, item) {
/// <summary>插入一项到数组中某位置</summary>
/// <param name="array" type="Array"></param>
/// <param name="index"></param>
/// <param name="item"></param>
}

Array.parse = function(value) {
/// <summary>解析字符串为数组</summary>
/// <param name="value"></param>
/// <returns type="Array elementMayBeNull="true""></returns>
}

Array.remove = function(array, item) {
/// <summary>移除数组中的一项内容</summary>
/// <param name="array" type="Array"></param>
/// <param name="item"></param>
/// <returns type="Boolean"></returns>
}

Array.removeAt = function(array, index) {
/// <summary>移除数组中的特定位置的一项</summary>
/// <param name="array" type="Array"></param>
/// <param name="index"></param>
}

Date.dateTimeToStr = function(dateTime) {
/// <summary>日期时间转为标准字符串</summary>
/// <param name="dateTime"></param>
/// <returns type="String"></returns>
}

Date.dateToStr = function(date) {
/// <summary>日期转为标准字符串</summary>
/// <param name="date" type="Date"></param>
/// <returns type="String"></returns>
}

Date.getDaysBetween = function(d1, d2) {
/// <summary>返回日期d1-d2之间的天数</summary>
/// <param name="d1"></param>
/// <param name="d2"></param>
}

Date.strToDate = function(str) {
/// <summary>标准字符串转为日期</summary>
/// <param name="str" type="String"></param>
/// <returns type="Date"></returns>
}

Date.strToDateTime = function(str) {
/// <summary>标准字符串转为日期时间</summary>
/// <param name="str" type="String"></param>
/// <returns type="Date"></returns>
}

Date.strToTime = function(str) {
/// <summary>标准字符串转为时间转</summary>
/// <param name="str" type="String"></param>
/// <returns type="Date"></returns>
}

Date.timeToStr = function(time) {
/// <summary>时间转为标准字符串</summary>
/// <param name="time"></param>
/// <returns type="String"></returns>
}

Date.prototype = {
    format: function(format) {
    /// <summary>日期格式化为字符串</summary>
    /// <param name="format" type="String"></param>
    /// <returns type="String"></returns>
    }
}


Error.abort = function(message) {
/// <summary>Abort异常用于终止调用堆栈，但不提示</summary>
/// <param name="message" type="String"></param>
/// <returns type="Error"></returns>
}

Error.invalidOperation = function(message) {
/// <summary>返回一个 非法操作 异常实例</summary>
/// <param name="message" type="String"></param>
/// <returns type="Error"></returns>
}

Function.createDelegate = function(instance, method) {
/// <summary>转化一个方法为事件方法</summary>
/// <param name="instance"></param>
/// <param name="method" type="Function"></param>
/// <returns type="Function"></returns>
}

Math.roundTo = function(number, digit) {
/// <summary>保留到特定小数点位数如
/// Math.roundTo(1.235, 2) -&gt; 1.24
/// Math.roundTo(1.3, 0) -&gt; 1
/// Math.roundTo(125, -1) -&gt; 130</summary>
/// <param name="number"></param>
/// <param name="digit"></param>
}

Number.parse = function(value) {
/// <summary>解析字符串为 Number</summary>
/// <param name="value" type="String"></param>
/// <returns type="Number"></returns>
}

Number.prototype = {
    format: function(format, displayNumberGroupSeparator) {
    /// <summary>根据一个格式格式化为字符串</summary>
    /// <param name="format" type="String"></param>
    /// <param name="displayNumberGroupSeparator"></param>
    /// <returns type="String"></returns>
    },

    toHex: function(width) {
    /// <summary>转化为16进制字符串，如果指定width，则前面补0到固定长度</summary>
    /// <param name="width"></param>
    }
}


Object.clone = function(instance) {
/// <summary>返回对象的深度克隆</summary>
/// <param name="instance" type="Object"></param>
/// <returns type="Object"></returns>
}

Object.copyTo = function(from, to) {
/// <summary>拷贝一个对象的所有属性值到另一个中对象中</summary>
/// <param name="from" type="Object"></param>
/// <param name="to" type="Object"></param>
}

Object.getTypeName = function(instance) {
/// <summary>返回对象的类型名</summary>
/// <param name="instance"></param>
/// <returns type="String"></returns>
}

String.format = function(format, args) {
/// <summary>字符串格式化，格式 format 参考.NET文档</summary>
/// <param name="format"></param>
/// <param name="args"></param>
/// <returns type="String"></returns>
}

String.prototype = {
    endsWith: function(suffix) {
    /// <summary>判断字符串后面是否 suffix</summary>
    /// <param name="suffix"></param>
    /// <returns type="String"></returns>
    },

    startsWith: function(prefix) {
    /// <summary>判断字符串前面是否 prefix</summary>
    /// <param name="prefix"></param>
    /// <returns type="String"></returns>
    },

    trim: function() {
    /// <summary>去掉字符串前后的所有空格，等效于Delphi的Trim</summary>
    /// <returns type="String"></returns>
    },

    trimEnd: function() {
    /// <summary>去掉字符串后面的所有空格，等效于Delphi的TrimRight</summary>
    /// <returns type="String"></returns>
    },

    trimStart: function() {
    /// <summary>去掉字符串前面的所有空格，等效于Delphi的TrimLeft</summary>
    /// <returns type="String"></returns>
    }
}


window.Type = Function;

Type.registerNamespace = function(namespacePath) {
/// <summary>注册命名空间</summary>
/// <param name="namespacePath" type="String"></param>
}

Type.prototype = {
    callBaseMethod: function(instance, name, baseArguments) {
    /// <summary>调用基类方法</summary>
    /// <param name="instance"></param>
    /// <param name="name" type="String"></param>
    /// <param name="baseArguments" type="Array"></param>
    },

    initializeBase: function(instance, baseArguments) {
    /// <summary>调用基类的 constructor</summary>
    /// <param name="instance"></param>
    /// <param name="baseArguments" type="Array"></param>
    },

    isInstanceOfType: function(instance) {
    /// <summary>返回实例是否当前类型</summary>
    /// <param name="instance"></param>
    /// <returns type="Boolean"></returns>
    },

    registerClass: function(typeName, baseType, interfaceTypes) {
    /// <summary>注册类继承以及接口实现</summary>
    /// <param name="typeName" type="String"></param>
    /// <param name="baseType" type="Type"></param>
    /// <param name="interfaceTypes" type="Type"></param>
    /// <returns type="Type"></returns>
    }
}


window.$addHandler = function(element, eventName, handler) {
/// <summary>添加事件</summary>
/// <param name="element"></param>
/// <param name="eventName" type="String"></param>
/// <param name="handler" type="Function"></param>
}

window.$createDelegate = function(instance, method) {
/// <summary>转化一个方法为事件方法</summary>
/// <param name="instance"></param>
/// <param name="method" type="Function"></param>
/// <returns type="Function"></returns>
}

window.asButton = function(obj) {
/// <summary>按钮</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.Button"></returns>
}

window.asCancelEventArgs = function(obj) {
/// <summary>支持取消的事件的参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.CancelEventArgs"></returns>
}

window.asCheckBox = function(obj) {
/// <summary>CheckBox</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.CheckBox"></returns>
}

window.asEdit = function(obj) {
/// <summary>编辑控件</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.CustomEdit"></returns>
}

window.asEnterPressEventArgs = function(obj) {
/// <summary>OnEnterPress 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.EnterPressEventArgs"></returns>
}

window.asForm = function(obj) {
/// <summary>表单</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Form"></returns>
}

window.asGrid = function(obj) {
/// <summary>Grid</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.Grid"></returns>
}

window.asGridCellBeginEditEventArgs = function(obj) {
/// <summary>Grid中 OnCellBeginEdit 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridCellBeginEditEventArgs"></returns>
}

window.asGridCellRenderingEventArgs = function(obj) {
/// <summary>Grid中 OnCellRendering 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridCellRenderingEventArgs"></returns>
}

window.asGridColumnChangeEventArgs = function(obj) {
/// <summary>Grid中列的 OnChange 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridColumnChangeEventArgs"></returns>
}

window.asGridGetDisplayTextEventArgs = function(obj) {
/// <summary>Grid中列的 OnGetDisplayText 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridGetDisplayTextEventArgs"></returns>
}

window.asGridGetRowNoEventArgs = function(obj) {
/// <summary>Grid中 OnGetRowNo 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridGetRowNoEventArgs"></returns>
}

window.asGridNewRecordEventArgs = function(obj) {
/// <summary>Grid中 OnNewRecord 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridNewRecordEventArgs"></returns>
}

window.asGridRowClickEventArgs = function(obj) {
/// <summary>Grid中 OnRowClick 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.GridRowClickEventArgs"></returns>
}

window.asImage = function(obj) {
/// <summary>图片</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.Image"></returns>
}

window.asLabel = function(obj) {
/// <summary>标签</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.Label"></returns>
}

window.asPage = function(obj) {
/// <summary>页面动作</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.PageAction"></returns>
}

window.asSelectorEdit = function(obj) {
/// <summary>选择编辑控件</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.SelectorEdit"></returns>
}

window.asSelectorSelectedEventArgs = function(obj) {
/// <summary>OnSelectorSelected 事件参数</summary>
/// <param name="obj" type="Object"></param>
/// <returns type="Sys.UI.Controls.SelectorSelectedEventArgs"></returns>
}

window.asyncCall = function(method) {
/// <summary>异步调用，即 window.setTimeout(method, 0);</summary>
/// <param name="method" type="Function"></param>
}

window.getClientBounds = function() {
/// <summary>获取窗口客户区域尺寸</summary>
}

window.SysConsts = function() {
/// <summary>系统常量</summary>
}

Sys = function() {
/// <summary>Sys 命名空间</summary>
}

Sys._Debug = function() {
/// <summary>Sys命名空间
/// 调试辅助类</summary>
}

Sys._Debug.prototype = {
    log: function(text, forceOutput) {
    /// <summary>输出日志，如果没有在测试状态则调用该方法没有任何作用</summary>
    /// <param name="text"></param>
    /// <param name="forceOutput"></param>
    },

    traceDump: function(object, name, sb) {
    /// <summary>跟踪输出一个对象（遍历其中的内容），同时指定一个名字</summary>
    /// <param name="object"></param>
    /// <param name="name" type="String"></param>
    /// <param name="sb" type="Sys.StringBuilder"></param>
    }
}


Sys.CancelEventArgs = function() {
/// <summary>空事件参数
/// 支持取消的事件的参数</summary>
}

Sys.CancelEventArgs.prototype = {
    get_cancel: function() {
    /// <summary>是否取消</summary>
    /// <returns type="Boolean">是否取消</returns>
    },
    set_cancel: function(value) {
    /// <summary>是否取消</summary>
    /// <param name="value" type="Boolean">值</param>
    }
}


Sys.Component = function() {
/// <summary>控件基类</summary>
}

Sys.Component.prototype = {
    get_form: function() {
    /// <summary>控件的表单</summary>
    /// <returns type="Sys.UI.CustomForm">控件的表单</returns>
    },

    get_id: function() {
    /// <summary>完整ID</summary>
    /// <returns type="String">完整ID</returns>
    },

    get_isDisposed: function() {
    /// <summary>是否已经被释放</summary>
    /// <returns type="Boolean">是否已经被释放</returns>
    }
}


Sys.EventArgs = function() {
/// <summary>事件参数</summary>
}

Sys.Hashtable = function() {
/// <summary>Hashtable 类</summary>
}

Sys.Hashtable.prototype = {
    get_count: function() {
    /// <summary>项数</summary>
    },

    get_data: function() {
    /// <summary>数据</summary>
    },

    get_item: function() {
    /// <summary>取项
    /// 设置项</summary>
    },
    set_item: function(value) {
    /// <summary>取项
    /// 设置项</summary>
    /// <param name="value">值</param>
    },

    add: function(key, value) {
    /// <summary>添加项</summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    },

    clear: function() {
    /// <summary>清除所有内容</summary>
    },

    isEmpty: function() {
    /// <summary>返回集合是否空</summary>
    /// <returns type="Boolean"></returns>
    },

    toString: function(separator) {
    /// <summary>返回字符串内容</summary>
    /// <param name="separator" type="String"></param>
    /// <returns type="String"></returns>
    }
}


Sys.StringBuilder = function(initialText) {
/// <summary>快速字符串处理类</summary>
/// <param name="initialText"></param>
}

Sys.StringBuilder.prototype = {
    append: function(text) {
    /// <summary>添加内容</summary>
    /// <param name="text"></param>
    /// <returns type="Sys.StringBuilder"></returns>
    },

    appendLine: function(text) {
    /// <summary>添加行</summary>
    /// <param name="text"></param>
    /// <returns type="Sys.StringBuilder"></returns>
    },

    clear: function() {
    /// <summary>清除所有内容</summary>
    /// <returns type="Sys.StringBuilder"></returns>
    },

    isEmpty: function() {
    /// <summary>返回内容是否空</summary>
    /// <returns type="Boolean"></returns>
    },

    toString: function(separator) {
    /// <summary>返回字符串内容</summary>
    /// <param name="separator" type="String"></param>
    /// <returns type="String"></returns>
    }
}


Sys.StringUtils = function() {
/// <summary>StringUtils 类</summary>
}

Sys.StringUtils.getPinyinCode = function(str, lowerCaseMode, maxLength) {
/// <summary>取汉字简拼，其中如果是多音字的则取第一个音，返回的都是小写字母
/// lowerCaseMode 不传则只处理汉字，其它内容保持不变；传 true 则生成时会去掉符号、全角字母数字转成半角小写字母
/// maxLength 不传则返回全部汉字的简拼</summary>
/// <param name="str"></param>
/// <param name="lowerCaseMode"></param>
/// <param name="maxLength"></param>
}

Sys.Timer = function() {
/// <summary></summary>
}

Sys.Timer.prototype = {
    get_enabled: function() {
    /// <summary>默认没有启用</summary>
    /// <returns type="Boolean">默认没有启用</returns>
    },
    set_enabled: function(value) {
    /// <summary>默认没有启用</summary>
    /// <param name="value" type="Boolean">值</param>
    },

    get_interval: function() {
    /// <summary>单位：毫秒</summary>
    /// <returns type="Number">单位：毫秒</returns>
    },
    set_interval: function(value) {
    /// <summary>单位：毫秒</summary>
    /// <param name="value" type="Number">值</param>
    },

    add_tick: function(handler) {
    /// <summary></summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_tick: function(handler) {
    /// <summary></summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    restartTimer: function() {
    /// <summary>重启定时器</summary>
    }
}


Sys.UI = function() {
/// <summary>Sys.UI 命名空间</summary>
}

Sys.UI._CommonToolkit = function() {
/// <summary>公共函数库</summary>
}

Sys.UI._CommonToolkit.prototype = {
    alert: function(message) {
    /// <summary>固定加前缀"【系统提示】\n\n"的alert</summary>
    /// <param name="message"></param>
    },

    checkAlert: function(condition, message) {
    /// <summary>condition为true，则提示message；返回condition值</summary>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    },

    checkError: function(condition, message) {
    /// <summary>condition为true，则终止调用堆栈，有message则提示</summary>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    },

    closeAllMDIForms: function(sender, onFilter, onEnd) {
    /// <summary>关闭当前打开的所有MDI表单，sender 传控件或表单实例</summary>
    /// <param name="sender"></param>
    /// <param name="onFilter"></param>
    /// <param name="onEnd"></param>
    },

    confirm: function(message) {
    /// <summary>固定加前缀"【系统提示】\n\n"的confirm</summary>
    /// <param name="message"></param>
    },

    encodeUrlParam: function(param) {
    /// <summary>如果直接使用escape，则加号传到服务器端会变成空格</summary>
    /// <param name="param"></param>
    },

    findFocusedGrid: function(form) {
    /// <summary>如果表单的当前焦点在Grid则返回其实例，否则返回null</summary>
    /// <param name="form"></param>
    },

    formatNumber: function(value, decimalDigits, decimalScale, displayThousandSeperator, displayEmptyForZero) {
    /// <summary>参数：数值、保留小数位数、最多小数位数、是否显示逗号、零值是否显示为空；
    /// decimalDigits, decimalScale 参数只用其一，decimalDigits优先；
    /// 最后两个参数可选</summary>
    /// <param name="value"></param>
    /// <param name="decimalDigits"></param>
    /// <param name="decimalScale"></param>
    /// <param name="displayThousandSeperator"></param>
    /// <param name="displayEmptyForZero"></param>
    },

    getMainForm: function(thisForm) {
    /// <summary>返回主窗体；传入当前窗体</summary>
    /// <param name="thisForm"></param>
    },

    getMDIForms: function(sender) {
    /// <summary>返回当前打开的MDI表单，sender 传按钮或表单实例</summary>
    /// <param name="sender"></param>
    /// <returns type="Array"></returns>
    },

    getMDIPageCount: function(sender) {
    /// <summary>返回当前打开的MDI窗口页面数，sender 传按钮或表单实例</summary>
    /// <param name="sender"></param>
    /// <returns type="Number" integer="true"></returns>
    },

    navigateUrl: function(url, target) {
    /// <summary>打开一个地址，默认为在新窗口中打开</summary>
    /// <param name="url"></param>
    /// <param name="target"></param>
    },

    refresh: function(url) {
    /// <summary>刷新当前页</summary>
    /// <param name="url"></param>
    },

    setActiveMDIForm: function(form) {
    /// <summary>设置当前激活的表单，form 表单实例</summary>
    /// <param name="form"></param>
    },

    showModalForm: function(sender, url, width, height, onLoaded, onOk, onClose) {
    /// <summary>显示模态弹出窗口</summary>
    /// <param name="sender"></param>
    /// <param name="url"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="onLoaded"></param>
    /// <param name="onOk"></param>
    /// <param name="onClose"></param>
    },

    showPage: function(sender, url, caption, closeFirst, pageTimeout) {
    /// <summary>用主窗口的 SDIPanel 或 SDIPanel 来打开页面
    /// sender 传事件参数的按钮实例等
    /// caption 参数如果是对象类型，则认为是传参数给页面
    /// pageTimeout 可选，单位为 秒</summary>
    /// <param name="sender"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="closeFirst"></param>
    /// <param name="pageTimeout"></param>
    /// <returns type="Sys.UI.CustomForm 可能返回null，如果页面已经在MDIPanel中从而没有打开新页面时"></returns>
    }
}


Sys.UI.Control = function(element) {
/// <summary>控件基类</summary>
/// <param name="element"></param>
}

Sys.UI.Control.prototype = {
    get_id: function() {
    /// <summary>ID</summary>
    /// <returns type="String">ID</returns>
    },

    get_visible: function() {
    /// <summary>是否可见</summary>
    /// <returns type="Boolean">是否可见</returns>
    },
    set_visible: function(value) {
    /// <summary>是否可见</summary>
    /// <param name="value" type="Boolean">值</param>
    }
}


Sys.UI.CustomForm = function() {
/// <summary>表单基类</summary>
}

Sys.UI.CustomForm.prototype = {
    get_action: function() {
    /// <summary>取表单对应的 PageAction 实例</summary>
    },
    set_activeControl: function(value) {
    /// <summary>激活控件</summary>
    /// <param name="value">值</param>
    },

    get_caption: function() {
    /// <summary>窗口标题</summary>
    },
    set_caption: function(value) {
    /// <summary>窗口标题</summary>
    /// <param name="value">值</param>
    },

    get_childForms: function() {
    /// <summary>返回子表单列表</summary>
    },

    get_controls: function() {
    /// <summary>当前表单的控件数组</summary>
    },

    get_dataSource: function() {
    /// <summary>当前表单绑定的数据源</summary>
    },

    get_enabled: function() {
    /// <summary>是否可用，默认为 true</summary>
    },
    set_enabled: function(value) {
    /// <summary>是否可用，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_focusedControl: function() {
    /// <summary>返回当前拥有焦点的控件</summary>
    },

    get_pageParams: function() {
    /// <summary>取当前表单在打开时调用时传入的页面参数对象</summary>
    /// <returns type="Object">取当前表单在打开时调用时传入的页面参数对象</returns>
    },

    get_params: function() {
    /// <summary>Url 参数</summary>
    },

    get_parentForm: function() {
    /// <summary>父窗体</summary>
    /// <returns type="Sys.UI.CustomForm">父窗体</returns>
    },

    get_service: function() {
    /// <summary>取默认服务</summary>
    },

    add_change: function(handler) {
    /// <summary>变动事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_change: function(handler) {
    /// <summary>变动事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_close: function(handler) {
    /// <summary>关闭事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_close: function(handler) {
    /// <summary>关闭事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_closed: function(handler) {
    /// <summary>关闭后的事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_closed: function(handler) {
    /// <summary>关闭后的事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_loaded: function(handler) {
    /// <summary>加载后事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_loaded: function(handler) {
    /// <summary>加载后事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_notifyClose: function(handler) {
    /// <summary>通知关闭的事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_notifyClose: function(handler) {
    /// <summary>通知关闭的事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    close: function() {
    /// <summary>关闭</summary>
    },

    convertUrl: function(url) {
    /// <summary>转换地址，可以转换 ~/ 开头的绝对地址</summary>
    /// <param name="url"></param>
    },

    dataBind: function(data) {
    /// <summary>绑定数据</summary>
    /// <param name="data"></param>
    },

    focus: function() {
    /// <summary>激活当前页面定义的激活控件</summary>
    },

    getElement: function(id) {
    /// <summary>找DOM元素；id 应为不完整的 id
    /// 标签不存在则抛出异常</summary>
    /// <param name="id"></param>
    },

    print: function(control) {
    /// <summary>打印一个控件</summary>
    /// <param name="control"></param>
    },

    refresh: function(url, params) {
    /// <summary>刷新当前表单；可选指定新的地址和页面参数</summary>
    /// <param name="url"></param>
    /// <param name="params"></param>
    },

    saveData: function(ignoreValidation) {
    /// <summary>保存数据；ignoreValidation 可选</summary>
    /// <param name="ignoreValidation"></param>
    }
}


Sys.UI.Form = function(owner, width, height, left, top) {
/// <summary>表单</summary>
/// <param name="owner"></param>
/// <param name="width"></param>
/// <param name="height"></param>
/// <param name="left"></param>
/// <param name="top"></param>
}

Sys.UI.Form.prototype = {
    get_allowMove: function() {
    /// <summary>是否允许移动，默认为 true</summary>
    },
    set_allowMove: function(value) {
    /// <summary>是否允许移动，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_caption: function() {
    /// <summary>标题</summary>
    },
    set_caption: function(value) {
    /// <summary>标题</summary>
    /// <param name="value">值</param>
    },

    get_height: function() {
    /// <summary>高度</summary>
    },
    set_height: function(value) {
    /// <summary>高度</summary>
    /// <param name="value">值</param>
    },

    get_showCloseButton: function() {
    /// <summary>是否显示关闭按钮，默认为 true</summary>
    },
    set_showCloseButton: function(value) {
    /// <summary>是否显示关闭按钮，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_visible: function() {
    /// <summary>是否可见</summary>
    },
    set_visible: function(value) {
    /// <summary>是否可见</summary>
    /// <param name="value">值</param>
    },

    add_cancel: function(handler) {
    /// <summary>取消事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_cancel: function(handler) {
    /// <summary>取消事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_ok: function(handler) {
    /// <summary>确定事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_ok: function(handler) {
    /// <summary>确定事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    close: function() {
    /// <summary>关闭</summary>
    },

    show: function(url) {
    /// <summary>显示非模态窗口；如果传了url，则同时显示该页面；调用show方法不同于showModal，它不会禁用背后的窗口</summary>
    /// <param name="url"></param>
    },

    showModal: function(url, params) {
    /// <summary>显示页面为模态对话框</summary>
    /// <param name="url" type="String">页面地址</param>
    /// <param name="params" type="Object">参数</param>
    }
}


Sys.UI.FormCloseEventArgs = function() {
/// <summary>表单关闭事件参数</summary>
}

Sys.UI.FormCloseEventArgs.prototype = {
    get_canClose: function() {
    /// <summary>确定后是否允许关闭，默认为 true</summary>
    },
    set_canClose: function(value) {
    /// <summary>确定后是否允许关闭，默认为 true</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Key = function() {
/// <summary>键盘键定义 (
/// backspace     = 8
/// tab           = 9
/// enter         = 13
/// esc           = 27
/// space         = 32
/// pageUp        = 33
/// pageDown      = 34
/// end           = 35
/// home          = 36
/// left          = 37
/// up            = 38
/// right         = 39
/// down          = 40
/// del           = 127
/// )</summary>
}

Sys.UI.MessageBox = function() {
/// <summary>消息框</summary>
}

Sys.UI.MessageBox.alert = function(message, handler, handlerOwner) {
/// <summary></summary>
/// <param name="message"></param>
/// <param name="handler"></param>
/// <param name="handlerOwner"></param>
}

Sys.UI.MessageBox.ask = function(message, buttons, handler, handlerOwner) {
/// <summary></summary>
/// <param name="message"></param>
/// <param name="buttons"></param>
/// <param name="handler"></param>
/// <param name="handlerOwner"></param>
}

Sys.UI.MessageBox.confirm = function(message, handler, handlerOwner) {
/// <summary></summary>
/// <param name="message"></param>
/// <param name="handler"></param>
/// <param name="handlerOwner"></param>
}

Sys.UI.PageAction = function() {
/// <summary>页面动作</summary>
}

Sys.UI.PageAction.prototype = {
    get_service: function() {
    /// <summary>取服务实例；等同于 this.get_form().get_service()</summary>
    }
}


Sys.UI.Actions = function() {
/// <summary>Sys.UI.Actions 命名空间</summary>
}

Sys.UI.Actions.ControlAction = function() {
/// <summary>动作命名空间
/// 控件动作</summary>
}

Sys.UI.Actions.ControlAction.prototype = {

}


Sys.UI.Actions.GetReportMasterDataEventArgs = function(masterFields) {
/// <summary>getMasterData 事件参数</summary>
/// <param name="masterFields"></param>
}

Sys.UI.Actions.GetReportMasterDataEventArgs.prototype = {
    get_data: function() {
    /// <summary>数据</summary>
    },
    set_data: function(value) {
    /// <summary>数据</summary>
    /// <param name="value">值</param>
    },

    addMasterField: function(name, dataField, visible) {
    /// <summary>添加字段</summary>
    /// <param name="name"></param>
    /// <param name="dataField"></param>
    /// <param name="visible"></param>
    }
}


Sys.UI.Actions.Report = function() {
/// <summary>打印报表</summary>
}

Sys.UI.Actions.Report.prototype = {
    get_showDetailHeader: function() {
    /// <summary>是否显示明细头，默认为true</summary>
    },
    set_showDetailHeader: function(value) {
    /// <summary>是否显示明细头，默认为true</summary>
    /// <param name="value">值</param>
    },

    directPrint: function() {
    /// <summary>直接打印</summary>
    }
}


Sys.UI.Controls = function() {
/// <summary>Sys.UI.Controls 命名空间</summary>
}

Sys.UI.Controls._CheckBox = function(element) {
/// <summary>CheckBox、RadioButton 控件基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls._CheckBox.prototype = {
    get_checked: function() {
    /// <summary>是否选中</summary>
    },
    set_checked: function(value) {
    /// <summary>是否选中</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls._CheckBoxList = function(element) {
/// <summary>CheckBoxList、RadioButtonList 控件基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls._CheckBoxList.prototype = {
    get_items: function() {
    /// <summary>显示项目表</summary>
    /// <returns type="Array">显示项目表</returns>
    },
    set_items: function(value) {
    /// <summary>显示项目表</summary>
    /// <param name="value" type="Array">值</param>
    }
}


Sys.UI.Controls._ClientPanel = function(element) {
/// <summary>客户区基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls._ClientPanel.prototype = {

}


Sys.UI.Controls._FlatButtonFace = function(element) {
/// <summary>按钮基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls._FlatButtonFace.prototype = {

}


Sys.UI.Controls._MenuContainer = function(element) {
/// <summary>菜单相关控件
/// 菜单容器基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.BarcodePrint = function(element) {
/// <summary>条码打印</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.BarcodePrint.prototype = {
    config: function(barCodes) {
    /// <summary>配置</summary>
    /// <param name="barCodes"></param>
    },

    print: function(barCodes, copies) {
    /// <summary>打印</summary>
    /// <param name="barCodes"></param>
    /// <param name="copies"></param>
    },

    writeData: function(colTitles, colWidths, rowData) {
    /// <summary>写数据</summary>
    /// <param name="colTitles"></param>
    /// <param name="colWidths"></param>
    /// <param name="rowData"></param>
    }
}


Sys.UI.Controls.Button = function(element) {
/// <summary>按钮</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Button.prototype = {

}


Sys.UI.Controls.ButtonEdit = function(element) {
/// <summary>带按钮编辑框</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.ButtonEdit.prototype = {
    get_enterPressed: function() {
    /// <summary>是否敲回车进来的</summary>
    }
}


Sys.UI.Controls.Calendar = function(element) {
/// <summary>日历 控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Calendar.prototype = {

}


Sys.UI.Controls.Chart = function(element) {
/// <summary>图表</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Chart.prototype = {
    get_isValueVisible: function() {
    /// <summary>是否显示值，默认为 true</summary>
    },
    set_isValueVisible: function(value) {
    /// <summary>是否显示值，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_mode: function() {
    /// <summary>模式</summary>
    },
    set_mode: function(value) {
    /// <summary>模式</summary>
    /// <param name="value">值</param>
    },

    get_zoomFactor: function() {
    /// <summary>缩放比例</summary>
    },
    set_zoomFactor: function(value) {
    /// <summary>缩放比例</summary>
    /// <param name="value">值</param>
    },

    print: function() {
    /// <summary>屏幕打印</summary>
    },

    refresh: function(params) {
    /// <summary>刷新</summary>
    /// <param name="params"></param>
    }
}


Sys.UI.Controls.CheckBox = function(element) {
/// <summary>CheckBox 控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.CheckBox.prototype = {

}


Sys.UI.Controls.CheckBoxList = function(element) {
/// <summary>CheckBoxList 控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.CheckBoxList.prototype = {
    deselectAll: function() {
    /// <summary>设置全部不选中</summary>
    },

    selectAll: function() {
    /// <summary>设置全部选中</summary>
    }
}


Sys.UI.Controls.Column = function() {
/// <summary>列基类</summary>
}

Sys.UI.Controls.Column.prototype = {
    get_caption: function() {
    /// <summary>标签</summary>
    },
    set_caption: function(value) {
    /// <summary>标签</summary>
    /// <param name="value">值</param>
    },

    get_control: function() {
    /// <summary>当前编辑控件</summary>
    },

    get_dataField: function() {
    /// <summary>绑定字段</summary>
    },

    get_displayField: function() {
    /// <summary>显示字段</summary>
    },

    get_enabled: function() {
    /// <summary>是否可用</summary>
    },
    set_enabled: function(value) {
    /// <summary>是否可用</summary>
    /// <param name="value">值</param>
    },

    get_footer: function() {
    /// <summary>页脚内容</summary>
    },
    set_footer: function(value) {
    /// <summary>页脚内容</summary>
    /// <param name="value">值</param>
    },

    get_footerReplaceText: function() {
    /// <summary>页脚替换文本</summary>
    },
    set_footerReplaceText: function(value) {
    /// <summary>页脚替换文本</summary>
    /// <param name="value">值</param>
    },

    get_form: function() {
    /// <summary>所在表单</summary>
    /// <returns type="Sys.UI.CustomForm">所在表单</returns>
    },

    get_grid: function() {
    /// <summary>拥有者</summary>
    /// <returns type="Sys.UI.Controls.Grid">拥有者</returns>
    },

    get_index: function() {
    /// <summary>索引</summary>
    },

    get_isEditing: function() {
    /// <summary>是否正在编辑</summary>
    },

    get_owner: function() {
    /// <summary>拥有者，同 get_grid</summary>
    /// <returns type="Sys.UI.Controls.Grid">拥有者，同 get_grid</returns>
    },

    get_readOnly: function() {
    /// <summary>是否只读</summary>
    },
    set_readOnly: function(value) {
    /// <summary>是否只读</summary>
    /// <param name="value">值</param>
    },

    get_replaceText: function() {
    /// <summary>替换文本
    /// disableRefresh传true，则不立即刷新界面，后面可以在刷新数据时一起刷</summary>
    },
    set_replaceText: function(value) {
    /// <summary>替换文本
    /// disableRefresh传true，则不立即刷新界面，后面可以在刷新数据时一起刷</summary>
    /// <param name="value">值</param>
    },

    get_summaryValue: function() {
    /// <summary>合计值</summary>
    },

    get_useServerSum: function() {
    /// <summary>是否使用服务器端合计，默认为 true</summary>
    },
    set_useServerSum: function(value) {
    /// <summary>是否使用服务器端合计，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_value: function() {
    /// <summary>当前编辑值，供 change 事件调用</summary>
    },

    get_visible: function() {
    /// <summary>是否可见</summary>
    },
    set_visible: function(value) {
    /// <summary>是否可见</summary>
    /// <param name="value">值</param>
    },

    focus: function() {
    /// <summary>设置焦点</summary>
    },

    getDisplayText: function(value, text) {
    /// <summary>取显示文本</summary>
    /// <param name="value"></param>
    /// <param name="text"></param>
    }
}


Sys.UI.Controls.Container = function(element) {
/// <summary>控件命名空间
/// 输入控件基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Container.prototype = {
    get_enabled: function() {
    /// <summary>是否可用</summary>
    },
    set_enabled: function(value) {
    /// <summary>是否可用</summary>
    /// <param name="value">值</param>
    },

    get_hint: function() {
    /// <summary>提示</summary>
    },
    set_hint: function(value) {
    /// <summary>提示</summary>
    /// <param name="value">值</param>
    },

    get_label: function() {
    /// <summary>控件标签</summary>
    },
    set_label: function(value) {
    /// <summary>控件标签</summary>
    /// <param name="value">值</param>
    },

    get_reportVisible: function() {
    /// <summary>是否打印</summary>
    },
    set_reportVisible: function(value) {
    /// <summary>是否打印</summary>
    /// <param name="value">值</param>
    },

    get_tag: function() {
    /// <summary>用户数据</summary>
    },
    set_tag: function(value) {
    /// <summary>用户数据</summary>
    /// <param name="value">值</param>
    },

    get_textColor: function() {
    /// <summary>字体颜色</summary>
    },
    set_textColor: function(value) {
    /// <summary>字体颜色</summary>
    /// <param name="value">值</param>
    },

    add_blur: function(handler) {
    /// <summary>blur 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_blur: function(handler) {
    /// <summary>blur 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_click: function(handler) {
    /// <summary>click 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_click: function(handler) {
    /// <summary>click 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_focus: function(handler) {
    /// <summary>focus 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_focus: function(handler) {
    /// <summary>focus 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_keyPress: function(handler) {
    /// <summary>keypress 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_keyPress: function(handler) {
    /// <summary>keypress 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    add_keyUp: function(handler) {
    /// <summary>keyup 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_keyUp: function(handler) {
    /// <summary>keyup 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    click: function(e) {
    /// <summary>调用点击事件，enabled=false的直接返回</summary>
    /// <param name="e"></param>
    },

    focus: function(select) {
    /// <summary>设置焦点，并且默认为全选</summary>
    /// <param name="select"></param>
    }
}


Sys.UI.Controls.CustomButton = function(element) {
/// <summary>按钮基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.CustomButton.prototype = {
    get_text: function() {
    /// <summary>显示文本</summary>
    },
    set_text: function(value) {
    /// <summary>显示文本</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.CustomDropDownEdit = function(element) {
/// <summary>下拉编辑基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.CustomDropDownEdit.prototype = {

}


Sys.UI.Controls.CustomEdit = function(element) {
/// <summary>编辑控件基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.CustomEdit.prototype = {
    get_allowTags: function() {
    /// <summary>是否允许 HTML标签</summary>
    },
    set_allowTags: function(value) {
    /// <summary>是否允许 HTML标签</summary>
    /// <param name="value">值</param>
    },

    get_column: function() {
    /// <summary>嵌入式控件所在的列</summary>
    },

    get_dataField: function() {
    /// <summary>数据绑定字段</summary>
    },

    get_filterChars: function() {
    /// <summary>过滤字符表</summary>
    },
    set_filterChars: function(value) {
    /// <summary>过滤字符表</summary>
    /// <param name="value">值</param>
    },

    get_owner: function() {
    /// <summary>在grid中使用时返回 grid，否则返回 null</summary>
    },

    get_readOnly: function() {
    /// <summary>是否只读</summary>
    },
    set_readOnly: function(value) {
    /// <summary>是否只读</summary>
    /// <param name="value">值</param>
    },

    get_text: function() {
    /// <summary>显示文本</summary>
    },
    set_text: function(value) {
    /// <summary>显示文本</summary>
    /// <param name="value">值</param>
    },

    get_value: function() {
    /// <summary>最终值，可能返回默认值</summary>
    },
    set_value: function(value) {
    /// <summary>最终值，可能返回默认值</summary>
    /// <param name="value">值</param>
    },

    add_change: function(handler) {
    /// <summary>变动事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_change: function(handler) {
    /// <summary>变动事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    selectAll: function() {
    /// <summary>选中全部文本</summary>
    }
}


Sys.UI.Controls.CustomListBox = function(items) {
/// <summary>列表框基类</summary>
/// <param name="items"></param>
}

Sys.UI.Controls.CustomListBox.prototype = {
    get_items: function() {
    /// <summary>显示项目表</summary>
    /// <returns type="Array">显示项目表</returns>
    },
    set_items: function(value) {
    /// <summary>显示项目表</summary>
    /// <param name="value" type="Array">值</param>
    },

    get_selectedItem: function() {
    /// <summary>选中项数据</summary>
    },

    add_selected: function(handler) {
    /// <summary>选中事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_selected: function(handler) {
    /// <summary>选中事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    }
}


Sys.UI.Controls.CustomTextEdit = function(element) {
/// <summary>文件编辑框基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.CustomTextEdit.prototype = {

}


Sys.UI.Controls.DateColumn = function() {
/// <summary>日期列</summary>
}

Sys.UI.Controls.DateEdit = function(element) {
/// <summary>日期编辑</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.DateEdit.prototype = {

}


Sys.UI.Controls.DateLimitEdit = function(element) {
/// <summary>日期期限编辑</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.DateLimitEdit.prototype = {

}


Sys.UI.Controls.DateTimeColumn = function() {
/// <summary>日期时间列</summary>
}

Sys.UI.Controls.DateTimeColumn.prototype = {

}


Sys.UI.Controls.DateTimeEdit = function(element) {
/// <summary>日期时间编辑</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.DateTimeEdit.prototype = {

}


Sys.UI.Controls.DropDownColumn = function() {
/// <summary>下拉列</summary>
}

Sys.UI.Controls.DropDownColumn.prototype = {
    get_items: function() {
    /// <summary>显示项目表</summary>
    /// <returns type="Array">显示项目表</returns>
    },
    set_items: function(value) {
    /// <summary>显示项目表</summary>
    /// <param name="value" type="Array">值</param>
    }
}


Sys.UI.Controls.DropDownEdit = function(element) {
/// <summary>DropDownEdit 控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.DropDownEdit.prototype = {
    get_items: function() {
    /// <summary>下拉项目表</summary>
    /// <returns type="Number">下拉项目表</returns>
    },
    set_items: function(value) {
    /// <summary>下拉项目表</summary>
    /// <param name="value" type="Number">值</param>
    },

    get_selectedIndex: function() {
    /// <summary>当前选中项索引</summary>
    },
    set_selectedIndex: function(value) {
    /// <summary>当前选中项索引</summary>
    /// <param name="value">值</param>
    },

    get_selectedItem: function() {
    /// <summary>选中项数据</summary>
    }
}


Sys.UI.Controls.DynamicColumn = function() {
/// <summary>动态列</summary>
}

Sys.UI.Controls.DynamicColumn.prototype = {
    get_allowHandInput: function() {
    /// <summary>是否允许自由输入，默认为 false</summary>
    },
    set_allowHandInput: function(value) {
    /// <summary>是否允许自由输入，默认为 false</summary>
    /// <param name="value">值</param>
    },

    get_allowTags: function() {
    /// <summary>是否允许 HTML标签</summary>
    },
    set_allowTags: function(value) {
    /// <summary>是否允许 HTML标签</summary>
    /// <param name="value">值</param>
    },

    get_filterChars: function() {
    /// <summary>过滤字符表</summary>
    },
    set_filterChars: function(value) {
    /// <summary>过滤字符表</summary>
    /// <param name="value">值</param>
    },

    get_selectOnly: function() {
    /// <summary>是否只选</summary>
    },
    set_selectOnly: function(value) {
    /// <summary>是否只选</summary>
    /// <param name="value">值</param>
    },

    get_selectorPage: function() {
    /// <summary>选择器页面地址</summary>
    },
    set_selectorPage: function(value) {
    /// <summary>选择器页面地址</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.EnterPressEventArgs = function() {
/// <summary>OnEnterPress 事件参数</summary>
}

Sys.UI.Controls.EnterPressEventArgs.prototype = {
    get_focusNext: function() {
    /// <summary>选中后焦点是否下移，默认为 true</summary>
    },
    set_focusNext: function(value) {
    /// <summary>选中后焦点是否下移，默认为 true</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.FileUpload = function(element) {
/// <summary>文件上传</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.FileUpload.submit = function(callerControl, onSuccess) {
/// <summary>开始上传</summary>
/// <param name="callerControl"></param>
/// <param name="onSuccess"></param>
}

Sys.UI.Controls.FileUpload.prototype = {
    get_timeout: function() {
    /// <summary>超时，单位为秒，默认为 10 分钟</summary>
    },
    set_timeout: function(value) {
    /// <summary>超时，单位为秒，默认为 10 分钟</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.FlashChart = function(element) {
/// <summary>FlashChart</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.FlashChart.prototype = {
    get_mode: function() {
    /// <summary>模式</summary>
    },
    set_mode: function(value) {
    /// <summary>模式</summary>
    /// <param name="value">值</param>
    },

    get_zoomFactor: function() {
    /// <summary>缩放比例</summary>
    },
    set_zoomFactor: function(value) {
    /// <summary>缩放比例</summary>
    /// <param name="value">值</param>
    },

    postImage: function() {
    /// <summary>转存图片</summary>
    },

    refresh: function(params) {
    /// <summary>刷新</summary>
    /// <param name="params"></param>
    }
}


Sys.UI.Controls.Grid = function(element) {
/// <summary>Grid</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Grid.prototype = {
    get_allItems: function() {
    /// <summary>分页Grid，取所有数据，返回对象数组</summary>
    },

    get_columns: function() {
    /// <summary>返回所有列的数组</summary>
    },

    get_dataSource: function() {
    /// <summary>数据源对象数组</summary>
    /// <returns type="Array elementType="Object" mayBeNull="true" elementMayBeNull="false"">数据源对象数组</returns>
    },

    get_height: function() {
    /// <summary>高度</summary>
    },
    set_height: function(value) {
    /// <summary>高度</summary>
    /// <param name="value">值</param>
    },

    get_loadedItems: function() {
    /// <summary>分页非只读Grid，取已加载的数据，返回对象数组</summary>
    },

    get_maxRowCount: function() {
    /// <summary>最大行数，没有设置则默认为 defaultRowCount</summary>
    },

    get_modifyOnly: function() {
    /// <summary>是否只做修改</summary>
    },
    set_modifyOnly: function(value) {
    /// <summary>是否只做修改</summary>
    /// <param name="value">值</param>
    },

    get_mouseHoverRowData: function() {
    /// <summary>取鼠标悬停行的数据对象，没有悬停时返回 null</summary>
    },

    get_pager: function() {
    /// <summary>分页器，找不到则返回异常</summary>
    },

    get_pagerClientId: function() {
    /// <summary>分页器唯一ID</summary>
    },

    get_readOnly: function() {
    /// <summary>是否只读，默认为 true</summary>
    },
    set_readOnly: function(value) {
    /// <summary>是否只读，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_recordCount: function() {
    /// <summary>当前记录数</summary>
    },

    get_selectedItems: function() {
    /// <summary>已选中行数据</summary>
    },

    get_selectedRowData: function() {
    /// <summary>选中行的数据对象</summary>
    },

    get_selectedRowIndex: function() {
    /// <summary>当前选中行的索引</summary>
    },
    set_selectedRowIndex: function(value) {
    /// <summary>当前选中行的索引</summary>
    /// <param name="value">值</param>
    },

    get_textColor: function() {
    /// <summary>文本颜色</summary>
    },
    set_textColor: function(value) {
    /// <summary>文本颜色</summary>
    /// <param name="value">值</param>
    },

    get_visible: function() {
    /// <summary>是否可见</summary>
    },
    set_visible: function(value) {
    /// <summary>是否可见</summary>
    /// <param name="value">值</param>
    },

    appendRowData: function(data, disableSetAsSelected) {
    /// <summary>添加行数据（如果有分页，则先跳到最后一页再添加）
    /// disableSetAsSelected传true，则新行不设为当前行</summary>
    /// <param name="data"></param>
    /// <param name="disableSetAsSelected"></param>
    },

    checkCanAppend: function() {
    /// <summary>用于在 appendRowData 前检查，当前行数等于MaxRowCount时提示“已达到最大数据行数 ***”，并停止调用堆栈</summary>
    },

    clearSelectedRowData: function() {
    /// <summary>清空选中行的数据（并不删除行）</summary>
    },

    clearSortFlag: function() {
    /// <summary>清除排序标志，不影响已有数据</summary>
    },

    config: function(hideNotAllowConfigColumns) {
    /// <summary>显示列配置对话框</summary>
    /// <param name="hideNotAllowConfigColumns"></param>
    },

    dataBind: function(data) {
    /// <summary>数据绑定
    /// 不传data则绑定到当前表单提供的数据，否则直接手工绑定到 data</summary>
    /// <param name="data"></param>
    },

    deleteRow: function(rowIndex) {
    /// <summary>删除特定行</summary>
    /// <param name="rowIndex"></param>
    },

    deleteSelectedItems: function() {
    /// <summary>删除多选行，这里假设服务器数据源已将数据删除，否则结果会不正确</summary>
    },

    deleteSelectedRow: function() {
    /// <summary>删除选中行</summary>
    },

    deselectAll: function() {
    /// <summary>设置全部不选中</summary>
    },

    endEdit: function() {
    /// <summary>提交当前编辑值并退出编辑状态</summary>
    },

    filter: function() {
    /// <summary>显示过滤对话框</summary>
    },

    findColumn: function(name, visibleOnly) {
    /// <summary>根据字段名取列，不存在则返回 null</summary>
    /// <param name="name"></param>
    /// <param name="visibleOnly"></param>
    },

    findRow: function(propName, propValue) {
    /// <summary>查找行，找到则返回行索引 &gt;= 0，否则返回 -1</summary>
    /// <param name="propName"></param>
    /// <param name="propValue"></param>
    /// <returns type="Number" integer="true"></returns>
    },

    findRowData: function(rowIndex) {
    /// <summary>根据索引取得特定行的数据对象，无效的行索引则返回 null</summary>
    /// <param name="rowIndex"></param>
    },

    focus: function() {
    /// <summary>设置焦点</summary>
    },

    focusNextRowColumn: function(column) {
    /// <summary>焦点跳到选中行下一行的某一列
    /// column 可以传实例，也可以传 dataField</summary>
    /// <param name="column"></param>
    },

    focusSelectedRowColumn: function(column) {
    /// <summary>焦点跳到选中行的某一列
    /// column 可以传实例，也可以传 dataField</summary>
    /// <param name="column"></param>
    },

    getColumn: function(name) {
    /// <summary>根据字段名取列，不存在则抛出异常</summary>
    /// <param name="name"></param>
    /// <returns type="Sys.UI.Controls.Column"></returns>
    },

    getRowData: function(rowIndex) {
    /// <summary>根据索引取得特定行的数据对象，无效的行索引则抛出异常</summary>
    /// <param name="rowIndex"></param>
    },

    getSelectedRowSelected: function() {
    /// <summary>获取当前行是否选中</summary>
    },

    getSummaryValue: function(dataField) {
    /// <summary>取合计值，可以在 OnSummaryLoaded 事件中使用</summary>
    /// <param name="dataField"></param>
    },

    insertRow: function(rowIndex) {
    /// <summary>在rowIndex指定的行之前插入一空行，返回行索引</summary>
    /// <param name="rowIndex"></param>
    },

    insertRowData: function(rowIndex, data) {
    /// <summary>在rowIndex指定的行之前插入行数据</summary>
    /// <param name="rowIndex"></param>
    /// <param name="data"></param>
    },

    locateRow: function(propName, propValue) {
    /// <summary>定位行，成功则返回 &gt;= 0，否则返回 -1</summary>
    /// <param name="propName"></param>
    /// <param name="propValue"></param>
    /// <returns type="Number" integer="true"></returns>
    },

    modifyRowData: function(rowIndex, data) {
    /// <summary>根据索引更新特定行的数据</summary>
    /// <param name="rowIndex"></param>
    /// <param name="data"></param>
    },

    modifySelectedRowData: function(data) {
    /// <summary>修改选中行数据</summary>
    /// <param name="data"></param>
    },

    moveDownSelectedRow: function() {
    /// <summary>当前行下移</summary>
    },

    moveUpSelectedRow: function() {
    /// <summary>当前行上移</summary>
    },

    refresh: function(dataSource) {
    /// <summary>用新的数据刷新当前显示，没有传dataSource则用当前数据来刷新显示</summary>
    /// <param name="dataSource"></param>
    },

    resetLoadedItems: function() {
    /// <summary>复位已加载项；预置数据变动前必须先调用该方法，否则已经修改过的数据不能使用新的预置数据</summary>
    },

    saveData: function(ignoreValidation) {
    /// <summary>保存并自己验证</summary>
    /// <param name="ignoreValidation"></param>
    },

    selectAll: function() {
    /// <summary>设置全部选中</summary>
    },

    setRowBgColor: function(rowIndex, value) {
    /// <summary>设置行背景色</summary>
    /// <param name="rowIndex"></param>
    /// <param name="value"></param>
    },

    setRowColumnBgColor: function(rowIndex, column, value) {
    /// <summary>设置行中某一列背景色</summary>
    /// <param name="rowIndex"></param>
    /// <param name="column"></param>
    /// <param name="value"></param>
    },

    setRowColumnEnabled: function(rowIndex, column, enabled) {
    /// <summary>设置行中某一CheckBox列是否 enabled</summary>
    /// <param name="rowIndex"></param>
    /// <param name="column"></param>
    /// <param name="enabled"></param>
    },

    setRowEnabled: function(rowIndex, enabled) {
    /// <summary>设置某一行是否允许编辑</summary>
    /// <param name="rowIndex"></param>
    /// <param name="enabled"></param>
    },

    setRowFontColor: function(rowIndex, value) {
    /// <summary>设置行文字颜色</summary>
    /// <param name="rowIndex"></param>
    /// <param name="value"></param>
    },

    setRowHint: function(rowIndex, value) {
    /// <summary>设置行提示</summary>
    /// <param name="rowIndex"></param>
    /// <param name="value"></param>
    },

    setRowsHint: function(value) {
    /// <summary>给所有行设提示信息</summary>
    /// <param name="value"></param>
    },

    setSelectedRowSelected: function(selected) {
    /// <summary>设置当前行是否选中</summary>
    /// <param name="selected"></param>
    }
}


Sys.UI.Controls.GridCellBeginEditEventArgs = function(rowIndex, columnIndex, column) {
/// <summary>Grid中 OnCellBeginEdit 事件参数</summary>
/// <param name="rowIndex"></param>
/// <param name="columnIndex"></param>
/// <param name="column"></param>
}

Sys.UI.Controls.GridCellBeginEditEventArgs.prototype = {
    get_column: function() {
    /// <summary>当前列</summary>
    },

    get_columnIndex: function() {
    /// <summary>当前列索引</summary>
    },

    get_rowIndex: function() {
    /// <summary>当前行索引</summary>
    }
}


Sys.UI.Controls.GridCellRenderingEventArgs = function(rowIndex, columnIndex, column) {
/// <summary>Grid中 OnCellRendering 事件参数</summary>
/// <param name="rowIndex"></param>
/// <param name="columnIndex"></param>
/// <param name="column"></param>
}

Sys.UI.Controls.GridCellRenderingEventArgs.prototype = {
    get_bgColor: function() {
    /// <summary>背景颜色</summary>
    },
    set_bgColor: function(value) {
    /// <summary>背景颜色</summary>
    /// <param name="value">值</param>
    },

    get_column: function() {
    /// <summary>当前列</summary>
    },

    get_columnIndex: function() {
    /// <summary>当前列索引</summary>
    },

    get_fontColor: function() {
    /// <summary>字体颜色</summary>
    },
    set_fontColor: function(value) {
    /// <summary>字体颜色</summary>
    /// <param name="value">值</param>
    },

    get_rowIndex: function() {
    /// <summary>当前行索引</summary>
    }
}


Sys.UI.Controls.GridColumnChangeEventArgs = function(rowIndex, value, oldValue) {
/// <summary>Grid中列的 OnChange 事件参数</summary>
/// <param name="rowIndex"></param>
/// <param name="value"></param>
/// <param name="oldValue"></param>
}

Sys.UI.Controls.GridColumnChangeEventArgs.prototype = {
    get_focusColumn: function() {
    /// <summary>变动后焦点是不下移而换到另一列，默认为 null</summary>
    },
    set_focusColumn: function(value) {
    /// <summary>变动后焦点是不下移而换到另一列，默认为 null</summary>
    /// <param name="value">值</param>
    },

    get_oldValue: function() {
    /// <summary>老的值</summary>
    },

    get_rowIndex: function() {
    /// <summary>当前行索引</summary>
    },

    get_value: function() {
    /// <summary>值</summary>
    },
    set_value: function(value) {
    /// <summary>值</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.GridColumnSortEventArgs = function(column) {
/// <summary>Grid中 OnColumnSort 事件参数</summary>
/// <param name="column"></param>
}

Sys.UI.Controls.GridColumnSortEventArgs.prototype = {
    get_column: function() {
    /// <summary>当前列</summary>
    }
}


Sys.UI.Controls.GridGetDisplayTextEventArgs = function(rowIndex, value, text) {
/// <summary>Grid中列的 OnGetDisplayText 事件参数</summary>
/// <param name="rowIndex"></param>
/// <param name="value"></param>
/// <param name="text"></param>
}

Sys.UI.Controls.GridGetDisplayTextEventArgs.prototype = {
    get_rowIndex: function() {
    /// <summary>当前行索引</summary>
    },

    get_text: function() {
    /// <summary>显示文本</summary>
    },
    set_text: function(value) {
    /// <summary>显示文本</summary>
    /// <param name="value">值</param>
    },

    get_value: function() {
    /// <summary>当前值</summary>
    }
}


Sys.UI.Controls.GridGetLoadedItemsEventArgs = function(loadedItems, appendItemIndex, decimalField) {
/// <summary>Grid中 getLoadedItems 事件参数</summary>
/// <param name="loadedItems"></param>
/// <param name="appendItemIndex"></param>
/// <param name="decimalField"></param>
}

Sys.UI.Controls.GridGetLoadedItemsEventArgs.prototype = {
    modifyOrAppendItems: function(items, keyField) {
    /// <summary>往已加载项中修改或添加数据items，以keyField为关键字做匹配查找</summary>
    /// <param name="items"></param>
    /// <param name="keyField"></param>
    }
}


Sys.UI.Controls.GridGetRowNoEventArgs = function(rowNo, rowData) {
/// <summary>Grid中 OnGetRowNo 事件参数</summary>
/// <param name="rowNo"></param>
/// <param name="rowData"></param>
}

Sys.UI.Controls.GridGetRowNoEventArgs.prototype = {
    get_rowData: function() {
    /// <summary>当前行数据</summary>
    },

    get_rowNo: function() {
    /// <summary>行号</summary>
    },
    set_rowNo: function(value) {
    /// <summary>行号</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.GridMouseHoverEventArgs = function(isHover, clientX, clientY) {
/// <summary>Grid中 mouseHover 事件参数</summary>
/// <param name="isHover"></param>
/// <param name="clientX"></param>
/// <param name="clientY"></param>
}

Sys.UI.Controls.GridMouseHoverEventArgs.prototype = {
    get_clientX: function() {
    /// <summary>坐标X</summary>
    },

    get_clientY: function() {
    /// <summary>坐标Y</summary>
    }
}


Sys.UI.Controls.GridNewRecordEventArgs = function(data) {
/// <summary>Grid中 OnNewRecord 事件参数</summary>
/// <param name="data"></param>
}

Sys.UI.Controls.GridNewRecordEventArgs.prototype = {
    get_data: function() {
    /// <summary>当前数据</summary>
    }
}


Sys.UI.Controls.GridRowClickEventArgs = function(rowIndex, column) {
/// <summary>Grid中 OnRowClick 事件参数</summary>
/// <param name="rowIndex"></param>
/// <param name="column"></param>
}

Sys.UI.Controls.GridRowClickEventArgs.prototype = {
    get_column: function() {
    /// <summary>当前列</summary>
    },

    get_rowIndex: function() {
    /// <summary>当前行索引</summary>
    }
}


Sys.UI.Controls.GridRowDeletingEventArgs = function(rowIndex) {
/// <summary>Grid中 OnRowDeleting 事件参数</summary>
/// <param name="rowIndex"></param>
}

Sys.UI.Controls.GridRowDeletingEventArgs.prototype = {
    get_rowIndex: function() {
    /// <summary>当前行索引</summary>
    }
}


Sys.UI.Controls.HiddenField = function(element) {
/// <summary>隐藏域</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.HiddenField.prototype = {

}


Sys.UI.Controls.HtmlEditor = function(element) {
/// <summary>HtmlEditor</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.HtmlEditor.prototype = {

}


Sys.UI.Controls.Image = function(element) {
/// <summary>图片</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Image.prototype = {
    get_src: function() {
    /// <summary>图片地址</summary>
    },
    set_src: function(value) {
    /// <summary>图片地址</summary>
    /// <param name="value">值</param>
    },

    get_url: function() {
    /// <summary>图片资源地址</summary>
    },
    set_url: function(value) {
    /// <summary>图片资源地址</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.Label = function(element) {
/// <summary>标签</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Label.prototype = {
    get_dataField: function() {
    /// <summary>数据绑定字段</summary>
    },

    get_text: function() {
    /// <summary>文本</summary>
    },
    set_text: function(value) {
    /// <summary>文本</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.LargeToolButton = function(element) {
/// <summary>大工具条按钮</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.LargeToolButton.prototype = {
    get_icon: function() {
    /// <summary>图标</summary>
    },
    set_icon: function(value) {
    /// <summary>图标</summary>
    /// <param name="value">值</param>
    },

    get_text: function() {
    /// <summary>显示文本</summary>
    },
    set_text: function(value) {
    /// <summary>显示文本</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.LayoutGroup = function(element) {
/// <summary>布局</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.LayoutGroup.prototype = {
    get_caption: function() {
    /// <summary>标题</summary>
    },
    set_caption: function(value) {
    /// <summary>标题</summary>
    /// <param name="value">值</param>
    },

    swapDisplayOrder: function(rowIndex1, rowIndex2) {
    /// <summary>交换其中两个Panel的显示顺序</summary>
    /// <param name="rowIndex1"></param>
    /// <param name="rowIndex2"></param>
    }
}


Sys.UI.Controls.ListBox = function(element) {
/// <summary>ListBox</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.ListBox.prototype = {
    get_items: function() {
    /// <summary>显示项目表</summary>
    /// <returns type="Array">显示项目表</returns>
    },
    set_items: function(value) {
    /// <summary>显示项目表</summary>
    /// <param name="value" type="Array">值</param>
    },

    get_selectedIndex: function() {
    /// <summary>当前选中项索引</summary>
    },
    set_selectedIndex: function(value) {
    /// <summary>当前选中项索引</summary>
    /// <param name="value">值</param>
    },

    deselectAll: function() {
    /// <summary>设置全部不选中</summary>
    },

    selectAll: function() {
    /// <summary>设置全部选中</summary>
    }
}


Sys.UI.Controls.MediaPlayer = function(element) {
/// <summary>媒体播放</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.MediaPlayer.prototype = {
    get_src: function() {
    /// <summary>媒体文件地址</summary>
    },
    set_src: function(value) {
    /// <summary>媒体文件地址</summary>
    /// <param name="value">值</param>
    },

    get_url: function() {
    /// <summary>媒体文件资源地址</summary>
    },
    set_url: function(value) {
    /// <summary>媒体文件资源地址</summary>
    /// <param name="value">值</param>
    },

    play: function() {
    /// <summary>播放</summary>
    },

    stop: function(value) {
    /// <summary>停止</summary>
    /// <param name="value"></param>
    }
}


Sys.UI.Controls.Menu = function(element) {
/// <summary>菜单</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Menu.prototype = {

}


Sys.UI.Controls.MenuItemBase = function(element) {
/// <summary>菜单项基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.MenuItemBase.prototype = {
    get_param: function() {
    /// <summary>参数</summary>
    },

    get_text: function() {
    /// <summary>显示文本</summary>
    },
    set_text: function(value) {
    /// <summary>显示文本</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.MenuRoot = function(element) {
/// <summary>根菜单类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.MenuRoot.prototype = {

}


Sys.UI.Controls.NavBarItem = function(element) {
/// <summary>NavBarItem</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.NavBarItem.prototype = {

}


Sys.UI.Controls.NoFocusContainer = function(element) {
/// <summary>没有焦点的容器</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.NumberColumn = function() {
/// <summary>数值列</summary>
}

Sys.UI.Controls.NumberColumn.prototype = {

}


Sys.UI.Controls.NumberEdit = function(element) {
/// <summary>数值编辑</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.NumberEdit.prototype = {
    get_replaceText: function() {
    /// <summary>替换文本</summary>
    },
    set_replaceText: function(value) {
    /// <summary>替换文本</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.PageControl = function(element) {
/// <summary>PageControl类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.PageControl.prototype = {
    get_selectedTabIndex: function() {
    /// <summary>当前选中页索引</summary>
    },
    set_selectedTabIndex: function(value) {
    /// <summary>当前选中页索引</summary>
    /// <param name="value">值</param>
    },

    get_tabCount: function() {
    /// <summary>页数</summary>
    },

    get_tabHeaderHidden: function() {
    /// <summary>是否隐藏标签头，默认 false</summary>
    },
    set_tabHeaderHidden: function(value) {
    /// <summary>是否隐藏标签头，默认 false</summary>
    /// <param name="value">值</param>
    },

    add_selected: function(handler) {
    /// <summary>selected 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },
    remove_selected: function(handler) {
    /// <summary>selected 事件</summary>
    /// <param name="handler" type="Function">处理函数</param>
    },

    getTab: function(index) {
    /// <summary>按索引取 Tab</summary>
    /// <param name="index"></param>
    /// <returns type="Sys.UI.Controls.TabPage"></returns>
    }
}


Sys.UI.Controls.Pager = function(element) {
/// <summary>分页器</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Pager.prototype = {
    get_dataSource: function() {
    /// <summary>客户端绑定的数据源</summary>
    },

    get_isLoading: function() {
    /// <summary>是否在加载数据</summary>
    },

    get_pageCount: function() {
    /// <summary>页数</summary>
    },

    get_pageIndex: function() {
    /// <summary>当前页索引（从0开始）</summary>
    },

    get_pageItemIndex: function() {
    /// <summary>取当前页第一条记录的索引（从0开始）</summary>
    },

    get_preloadItemFields: function() {
    /// <summary>预加载字段表，逗号分隔</summary>
    },
    set_preloadItemFields: function(value) {
    /// <summary>预加载字段表，逗号分隔</summary>
    /// <param name="value">值</param>
    },

    get_queryParams: function() {
    /// <summary>当前查询参数</summary>
    },

    get_showAll: function() {
    /// <summary>是否显示素有记录</summary>
    /// <returns type="Boolean">是否显示素有记录</returns>
    },
    set_showAll: function(value) {
    /// <summary>是否显示素有记录</summary>
    /// <param name="value" type="Boolean">值</param>
    },

    dataBind: function(dataSource) {
    /// <summary>客户端绑定</summary>
    /// <param name="dataSource"></param>
    },

    init: function(itemCount) {
    /// <summary>分页器单独使用时，传入记录数以重新初始化</summary>
    /// <param name="itemCount"></param>
    },

    lastPage: function(onSuccess) {
    /// <summary>到最后一页</summary>
    /// <param name="onSuccess"></param>
    },

    refresh: function(params, onSuccess) {
    /// <summary>刷新：重新查询并跳到第一页，参数都是可选的</summary>
    /// <param name="params"></param>
    /// <param name="onSuccess"></param>
    },

    refreshPage: function(params, pageIndex, onSuccess) {
    /// <summary>重新查询，并显示某页；pageIndex不传则显示当前页；onSuccess参数是可选的</summary>
    /// <param name="params"></param>
    /// <param name="pageIndex"></param>
    /// <param name="onSuccess"></param>
    },

    refreshSummary: function() {
    /// <summary>刷新合计（在服务器端重新计算后客户端刷新显示）</summary>
    },

    reloadPage: function(pageIndex, onSuccess) {
    /// <summary>重新加载页，参数都可选；如果不指定 pageIndex 则重新加载当前页</summary>
    /// <param name="pageIndex"></param>
    /// <param name="onSuccess"></param>
    },

    resetPageIndex: function() {
    /// <summary>Pager单独使用时，调用该返回设置当前页索引的值，不会导致数据加载</summary>
    }
}


Sys.UI.Controls.PercentColumn = function() {
/// <summary>自动添加一个百分号的列</summary>
}

Sys.UI.Controls.PercentColumn.prototype = {

}


Sys.UI.Controls.PosPrint = function(element) {
/// <summary>POS打印</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.PosPrint.prototype = {
    config: function() {
    /// <summary>设置</summary>
    },

    hideConfigPage: function(pageIndex) {
    /// <summary>如果不用条码称设置，应调用 hideConfigPage(1)</summary>
    /// <param name="pageIndex"></param>
    },

    openCashBox: function() {
    /// <summary>开钱箱</summary>
    },

    screenOut: function(type, money) {
    /// <summary>客显屏 type=2 为合计</summary>
    /// <param name="type"></param>
    /// <param name="money"></param>
    }
}


Sys.UI.Controls.RadioButtonList = function(element) {
/// <summary>RadioButtonList 控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.RadioButtonList.prototype = {
    get_selectedIndex: function() {
    /// <summary>当前选中项索引</summary>
    },
    set_selectedIndex: function(value) {
    /// <summary>当前选中项索引</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.RowDeleteColumn = function() {
/// <summary>行删除列</summary>
}

Sys.UI.Controls.RowDeleteColumn.prototype = {

}


Sys.UI.Controls.Scheduler = function(element) {
/// <summary>日程</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.Scheduler.prototype = {
    get_mode: function() {
    /// <summary>模式</summary>
    },
    set_mode: function(value) {
    /// <summary>模式</summary>
    /// <param name="value">值</param>
    },

    get_selectedDate: function() {
    /// <summary>当前选中日期</summary>
    },
    set_selectedDate: function(value) {
    /// <summary>当前选中日期</summary>
    /// <param name="value">值</param>
    },

    refresh: function() {
    /// <summary>刷新</summary>
    },

    refreshAll: function() {
    /// <summary>刷新全部</summary>
    }
}


Sys.UI.Controls.SelectorBeforeSelectedEventArgs = function(edit, value) {
/// <summary>selectorBeforeSelected 事件参数</summary>
/// <param name="edit"></param>
/// <param name="value"></param>
}

Sys.UI.Controls.SelectorBeforeSelectedEventArgs.prototype = {
    get_value: function() {
    /// <summary>当前值</summary>
    }
}


Sys.UI.Controls.SelectorColumn = function() {
/// <summary>选择列</summary>
}

Sys.UI.Controls.SelectorColumn.prototype = {
    get_allowHandInput: function() {
    /// <summary>是否允许自由输入，默认为 false</summary>
    },
    set_allowHandInput: function(value) {
    /// <summary>是否允许自由输入，默认为 false</summary>
    /// <param name="value">值</param>
    },

    get_allowTags: function() {
    /// <summary>是否允许 HTML标签</summary>
    },
    set_allowTags: function(value) {
    /// <summary>是否允许 HTML标签</summary>
    /// <param name="value">值</param>
    },

    get_filterChars: function() {
    /// <summary>过滤字符表</summary>
    },
    set_filterChars: function(value) {
    /// <summary>过滤字符表</summary>
    /// <param name="value">值</param>
    },

    get_selectOnly: function() {
    /// <summary>是否只选</summary>
    },
    set_selectOnly: function(value) {
    /// <summary>是否只选</summary>
    /// <param name="value">值</param>
    },

    get_selectorPage: function() {
    /// <summary>选择器页面地址</summary>
    },
    set_selectorPage: function(value) {
    /// <summary>选择器页面地址</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.SelectorEdit = function(element) {
/// <summary>选择编辑控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.SelectorEdit.prototype = {
    get_allowHandInput: function() {
    /// <summary>是否允许自由输入，默认为 false</summary>
    },
    set_allowHandInput: function(value) {
    /// <summary>是否允许自由输入，默认为 false</summary>
    /// <param name="value">值</param>
    },

    get_displayField: function() {
    /// <summary>显示字段</summary>
    },

    get_selectOnly: function() {
    /// <summary>是否只选</summary>
    },
    set_selectOnly: function(value) {
    /// <summary>是否只选</summary>
    /// <param name="value">值</param>
    },

    get_selectorPage: function() {
    /// <summary>选择器页面地址</summary>
    },
    set_selectorPage: function(value) {
    /// <summary>选择器页面地址</summary>
    /// <param name="value">值</param>
    },

    get_selectorPageParams: function() {
    /// <summary>选择器页面参数</summary>
    },
    set_selectorPageParams: function(value) {
    /// <summary>选择器页面参数</summary>
    /// <param name="value">值</param>
    },

    get_textChanged: function() {
    /// <summary>是否做了键盘输入</summary>
    },
    set_textChanged: function(value) {
    /// <summary>是否做了键盘输入</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.SelectorEventArgs = function(form, selector) {
/// <summary>selector事件参数</summary>
/// <param name="form"></param>
/// <param name="selector"></param>
}

Sys.UI.Controls.SelectorEventArgs.prototype = {
    get_form: function() {
    /// <summary>选择对话框（表单）</summary>
    }
}


Sys.UI.Controls.SelectorSelectedEventArgs = function(form, selector) {
/// <summary>OnSelectorSelected 事件参数</summary>
/// <param name="form"></param>
/// <param name="selector"></param>
}

Sys.UI.Controls.SelectorSelectedEventArgs.prototype = {
    get_canClose: function() {
    /// <summary>选中后表单能否关闭，默认为 true</summary>
    },
    set_canClose: function(value) {
    /// <summary>选中后表单能否关闭，默认为 true</summary>
    /// <param name="value">值</param>
    },

    get_focusNext: function() {
    /// <summary>选中后焦点是否下移，默认为 true</summary>
    },
    set_focusNext: function(value) {
    /// <summary>选中后焦点是否下移，默认为 true</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.TabPage = function(element) {
/// <summary>Tab页</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.TabPage.prototype = {
    get_caption: function() {
    /// <summary>标题</summary>
    },
    set_caption: function(value) {
    /// <summary>标题</summary>
    /// <param name="value">值</param>
    },

    close: function() {
    /// <summary>关闭</summary>
    }
}


Sys.UI.Controls.TextColumn = function() {
/// <summary>文本列</summary>
}

Sys.UI.Controls.TextColumn.prototype = {
    get_allowTags: function() {
    /// <summary>是否允许HTML标签</summary>
    },
    set_allowTags: function(value) {
    /// <summary>是否允许HTML标签</summary>
    /// <param name="value">值</param>
    },

    get_filterChars: function() {
    /// <summary>过滤字符表</summary>
    },
    set_filterChars: function(value) {
    /// <summary>过滤字符表</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.TimeEdit = function(element) {
/// <summary>TimeEdit 控件</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.TimeEdit.prototype = {

}


Sys.UI.Controls.ToolImage = function(element) {
/// <summary>工具图片</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.ToolImage.prototype = {
    get_image: function() {
    /// <summary>图片</summary>
    },
    set_image: function(value) {
    /// <summary>图片</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.ToolItem = function(element) {
/// <summary>工具条项基类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.ToolItem.prototype = {
    get_param: function() {
    /// <summary></summary>
    }
}


Sys.UI.Controls.TreeNode = function(value) {
/// <summary>抽象树节点类</summary>
/// <param name="value"></param>
}

Sys.UI.Controls.TreeNode.prototype = {
    get_checked: function() {
    /// <summary>获取是否选中</summary>
    },

    get_data: function() {
    /// <summary>节点数据</summary>
    },
    set_data: function(value) {
    /// <summary>节点数据</summary>
    /// <param name="value">值</param>
    },

    get_text: function() {
    /// <summary>文本</summary>
    },

    get_value: function() {
    /// <summary>值</summary>
    },

    appendChild: function(childNode) {
    /// <summary>添加子节点</summary>
    /// <param name="childNode"></param>
    },

    focus: function() {
    /// <summary>设焦点</summary>
    }
}


Sys.UI.Controls.TreeView = function(element) {
/// <summary>TreeView 类</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.TreeView.prototype = {
    get_selectedNode: function() {
    /// <summary>当前选中节点</summary>
    /// <returns type="Sys.UI.Controls.TreeNode">当前选中节点</returns>
    },

    refresh: function(dataSource) {
    /// <summary>刷新</summary>
    /// <param name="dataSource"></param>
    }
}


Sys.UI.Controls.TreeViewGetNodeDisplayTextEventArgs = function(node) {
/// <summary>TreeView 中 getNodeDisplayText 事件参数</summary>
/// <param name="node"></param>
}

Sys.UI.Controls.TreeViewGetNodeDisplayTextEventArgs.prototype = {
    get_text: function() {
    /// <summary>文本</summary>
    },
    set_text: function(value) {
    /// <summary>文本</summary>
    /// <param name="value">值</param>
    }
}


Sys.UI.Controls.VerificationCode = function(element) {
/// <summary>验证码</summary>
/// <param name="element"></param>
}

Sys.UI.Controls.VerificationCode.prototype = {
    change: function() {
    /// <summary>更换验证码</summary>
    }
}


Type.prototype.extend = function(baseType) {
    if (!baseType) throw new Error("基类没有：" + this.toString());
    var basePrototype = baseType.prototype;
    for (var name in basePrototype) {
        this.prototype[name] = basePrototype[name];
    }
}

var $debug = new Sys._Debug();


Sys.CancelEventArgs.extend(Sys.EventArgs);
Sys.Timer.extend(Sys.Component);
Sys.UI.Control.extend(Sys.Component);
Sys.UI.CustomForm.extend(Sys.Component);
Sys.UI.FormCloseEventArgs.extend(Sys.EventArgs);
Sys.UI.PageAction.extend(Sys.Component);
Sys.UI.Form.extend(Sys.UI.CustomForm);
Sys.UI.Actions.ControlAction.extend(Sys.Component);
Sys.UI.Actions.GetReportMasterDataEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Actions.Report.extend(Sys.UI.Actions.ControlAction);
Sys.UI.Controls.Column.extend(Sys.Component);
Sys.UI.Controls.GridCellRenderingEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.GridGetDisplayTextEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.GridGetLoadedItemsEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.GridGetRowNoEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.GridMouseHoverEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.GridNewRecordEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.GridRowClickEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.SelectorEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls.TreeViewGetNodeDisplayTextEventArgs.extend(Sys.EventArgs);
Sys.UI.Controls._ClientPanel.extend(Sys.UI.Control);
Sys.UI.Controls.BarcodePrint.extend(Sys.UI.Control);
Sys.UI.Controls.Chart.extend(Sys.UI.Control);
Sys.UI.Controls.Container.extend(Sys.UI.Control);
Sys.UI.Controls.DateColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.DateTimeColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.DropDownColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.DynamicColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.EnterPressEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Controls.FileUpload.extend(Sys.UI.Control);
Sys.UI.Controls.FlashChart.extend(Sys.UI.Control);
Sys.UI.Controls.Grid.extend(Sys.UI.Control);
Sys.UI.Controls.GridCellBeginEditEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Controls.GridColumnChangeEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Controls.GridColumnSortEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Controls.GridRowDeletingEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Controls.NumberColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.PageControl.extend(Sys.UI.Control);
Sys.UI.Controls.Pager.extend(Sys.UI.Control);
Sys.UI.Controls.PosPrint.extend(Sys.UI.Control);
Sys.UI.Controls.RowDeleteColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.Scheduler.extend(Sys.UI.Control);
Sys.UI.Controls.SelectorBeforeSelectedEventArgs.extend(Sys.CancelEventArgs);
Sys.UI.Controls.SelectorColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls.SelectorSelectedEventArgs.extend(Sys.UI.Controls.SelectorEventArgs);
Sys.UI.Controls.TabPage.extend(Sys.UI.Control);
Sys.UI.Controls.TextColumn.extend(Sys.UI.Controls.Column);
Sys.UI.Controls._FlatButtonFace.extend(Sys.UI.Controls.Container);
Sys.UI.Controls.CustomEdit.extend(Sys.UI.Controls.Container);
Sys.UI.Controls.NoFocusContainer.extend(Sys.UI.Controls.Container);
Sys.UI.Controls.PercentColumn.extend(Sys.UI.Controls.NumberColumn);
Sys.UI.Controls.TreeView.extend(Sys.UI.Controls.Container);
Sys.UI.Controls._CheckBox.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls._CheckBoxList.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls._MenuContainer.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.Calendar.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.CustomButton.extend(Sys.UI.Controls._FlatButtonFace);
Sys.UI.Controls.CustomDropDownEdit.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.CustomListBox.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.CustomTextEdit.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.DateTimeEdit.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.HiddenField.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.HtmlEditor.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.Image.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.Label.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.LayoutGroup.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.ListBox.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.MediaPlayer.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.MenuItemBase.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.NavBarItem.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.NumberEdit.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.TimeEdit.extend(Sys.UI.Controls.CustomEdit);
Sys.UI.Controls.ToolItem.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.VerificationCode.extend(Sys.UI.Controls.NoFocusContainer);
Sys.UI.Controls.Button.extend(Sys.UI.Controls.CustomButton);
Sys.UI.Controls.ButtonEdit.extend(Sys.UI.Controls.CustomTextEdit);
Sys.UI.Controls.CheckBox.extend(Sys.UI.Controls._CheckBox);
Sys.UI.Controls.CheckBoxList.extend(Sys.UI.Controls._CheckBoxList);
Sys.UI.Controls.DateEdit.extend(Sys.UI.Controls.CustomDropDownEdit);
Sys.UI.Controls.DropDownEdit.extend(Sys.UI.Controls.CustomDropDownEdit);
Sys.UI.Controls.LargeToolButton.extend(Sys.UI.Controls.ToolItem);
Sys.UI.Controls.Menu.extend(Sys.UI.Controls._MenuContainer);
Sys.UI.Controls.MenuRoot.extend(Sys.UI.Controls.MenuItemBase);
Sys.UI.Controls.RadioButtonList.extend(Sys.UI.Controls._CheckBoxList);
Sys.UI.Controls.ToolImage.extend(Sys.UI.Controls.ToolItem);
Sys.UI.Controls.DateLimitEdit.extend(Sys.UI.Controls.DateEdit);
Sys.UI.Controls.SelectorEdit.extend(Sys.UI.Controls.ButtonEdit);
