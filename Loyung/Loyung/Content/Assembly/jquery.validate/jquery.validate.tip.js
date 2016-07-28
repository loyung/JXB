/*
*创建人：刘自洋
*创建时间：2016-04-20
*说明：该文件对jquery.validate几种验证事件进行重配置，以便于做消息Tip提醒。是validate与poshytip结合的关键
*/
$.validator.setDefaults({
    /*关闭键盘输入时的实时校验*/
    onkeyup: function (element) {
        //$(element).poshytip({ content: "键盘输入", showOn: 'elemshow' });
    },
    /*校验成功/
    success: function (element) {
        //$(element).poshytip({ content: "校验成功", showOn: 'elemshow' });
    },
    /*获得焦点后执行*/
    onfocusin: function (element) {
        this.lastActive = element;
        this.addWrapper(this.errorsFor(element)).hide();
        var focusintip = $(element).attr('focusin');
        if (focusintip && $(element).parent().children(".focusin").length === 0) {
            //$(element).parent().poshytip({ content: focusintip, showOn: 'elemshow' });//tip光标选中后提示
        }
      
        // Hide error label and remove error class on focus if enabled
        if (this.settings.focusCleanup) {
            if (this.settings.unhighlight) {
                this.settings.unhighlight.call(this, element, this.settings.errorClass, this.settings.validClass);
            }
            this.hideThese(this.errorsFor(element));
        }
    },
    /*焦点离开*/
    onfocusout: function (element) {
        $(element).valid();
        //if ($(element).text() != "")
        //    $(element).parent().poshytip({ showOn: 'elemshow', content: $(element).text() });
        if ( !this.checkable( element ) && ( element.name in this.submitted || !this.optional( element ) ) ) {
            this.element( element );
        }
    },
    errorPlacement: function (error, element) {
        if ($(error).text() != "") {
            $(element).parent().poshytip({ content: $(error).text(), showOn: 'elemshow' });
        }
    }
});