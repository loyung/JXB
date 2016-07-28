/*
*创建时间：2016-05-24
*创建人：刘自洋
*说明：存放页面控件动态数据绑定
*/

/*绑定select标签*/
//此URL返回带有value和text结果json
(function ($) {
    ///绑定页面下拉框,appendRoot(bool)是否追加根节点,存在根节点时指定默认显示名称
    $.fn.bindSelect = function (targetUrl, appendRoot, RootName) {
        var $this = this;
        $.ajax({
            type: "POST",
            async:false,
            dataType: "json",
            url: targetUrl,
            success: function (msg) {
                if (appendRoot) {
                    var root = document.createElement('option');
                    root.value = -1;
                    root.innerHTML = RootName;
                    $($this).append(root);
                }
                for (var item in msg) {
                    var option = document.createElement('option');
                    option.value = msg[item].value;
                    option.innerHTML = msg[item].text;
                    $($this).append(option);
                }
            }
        });
       
        //绑定select
        $("select[selectvalue]").each(function (index, item) {
            var $selectvalue = $(item).attr("selectvalue");
            $(item).find("option[value='" + $selectvalue + "']").attr("selected", "selected");
        });
    };
    ///获取form表单并将转化为json对象
    $.fn.formToJson = function () {
        var $this = this;
        var formdata = $(this).serialize();
        formdata = decodeURIComponent(formdata, true);
        formdata = formdata.replace(/&/g, "\",\"");
        formdata = formdata.replace(/=/g, "\":\"");
        formdata = "{\"" + formdata + "\"}";
        return formdata;
    }

    ///获取form表单并将转化字符串追加至json对象
    $.fn.formAppendJson = function (json) {
        var postData = "";
        if (json != null) {
            for (var item in json) {
                postData += item + "=" + json[item] + "&";
            }
        }
        postData += $(this).serialize();
        return postData;
    }
    //查找绑定控件值
    $.fn.BindControl = function (TargetValue) {
        if ($(this)[0] == null || $(this)[0]==undefined)
            return;
        if (TargetValue != null && TargetValue != undefined && TargetValue.toString().indexOf('/Date(') != -1)
        {//时间戳【/Date(740073600000)/】
            var datetime = new Date(TargetValue);
            TargetValue = stamp2datetime(TargetValue);
        }
        var TargetElement = $(this);//获取当前对象
        var TargetName = $(this)[0].tagName//获取当前元素的标记类型
        switch (TargetName.toLocaleLowerCase())
        {
            case "select":
                $(this).attr("selectvalue", TargetValue);
                $(this).find("option[value='" + TargetValue + "']").attr("selected", "selected");
                break;
            case "textarea":
                $(this).val(TargetValue);
                break;
            case "input":
                if (TargetElement.is(':radio'))
                {
                    TargetElement.attr("radiovalue", TargetValue).nextAll().attr("radiovalue", TargetValue);
                    //选中当前对应项
                    $("[radiovalue='" + TargetValue + "']").each(function (index, item) {
                        if ($(item).val() == $(item).attr("radiovalue"))
                        { $(item).attr("checked",true)}
                    });
                }
                else if (TargetElement.is(':checkbox'))
                {
                    TargetElement.attr("checkboxvalue", TargetValue);
                }
                $(this).val(TargetValue);
                break;

        }
    }
})(jQuery)
//绑定表单数据需(使用前引用JQuery)
$(function () {
    //绑定radio
    $("input[type=radio][radiovalue]").each(function (index, item) {
        var $selectvalue = $(item).attr("radiovalue");
        var $thisvalue = $(item).val();
        if ($thisvalue == $selectvalue) {
            $(item).attr("checked", true);
        }
    });
    
});
