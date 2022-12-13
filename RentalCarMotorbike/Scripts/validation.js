function Validator(formSeclector) {
    var formRules = {}; 
    var _this = this;

    var validatorRules = {
        required: function (value) {
            return value ? undefined : 'Please enter this field';
        },
        email: function (value) {
            var regex = /^([A-Za-z0-9_\-.])+@([A-Za-z0-9_\-.])+\.([A-Za-z]{2,4})$/;
            return regex.test(value) ? undefined : 'Must be email';
        },
        min: function (min) {
            return function (value) {
                return value.length >= min ? undefined : `Password at least ${min} characters`;
            }
        },
        phoneLength: function (min) {
            return function (value) {
                return value.length >= min ? undefined : `Contact number must be ${min}`;
            }
        },
    };

    // Lấy ra form element trong DOM theo formSelector
    var formElement = document.querySelector(formSeclector);
    
    if(formElement){

        var inputs = formElement.querySelectorAll('[name][rule]');

        for(var input of inputs){

            var rules = input.getAttribute('rule').split('|');

            for(var rule of rules){

                var isRuleHasValue = rule.includes(':');
                var ruleInfo;
                if(isRuleHasValue){
                    ruleInfo = rule.split(':');

                    rule = ruleInfo[0];
                }

                var ruleFunc = validatorRules[rule];

                if(isRuleHasValue){
                    ruleFunc = ruleFunc(ruleInfo[1]);
                }

                if(Array.isArray(formRules[input.name])){
                    formRules[input.name].push(ruleFunc);
                }else{
                    formRules[input.name] = [ruleFunc];
                }

                // Lắng nghe sự kiện để validate(blur,input,....)
    
                input.onblur = handleValidate;
                input.oninput = handleClearError;
            }

            // Hàm thực hiện Validator
            function handleValidate(event){
                var rules = formRules[event.target.name];

                var errorMessage;

                for(var rule of rules) {
                    errorMessage =  rule(event.target.value);
                    if(errorMessage) break;
                }

                if(errorMessage){
                    var formGroup = event.target.closest('.form-group');
                    
                    if(formGroup){
                        var formMessage = formGroup.querySelector('.form-message');

                        if(formMessage){
                            formMessage.innerText = errorMessage;
                            formGroup.classList.add('invalid');
                        }
                    }
                }

                return !errorMessage;
            }

            // Hàm clear message lỗi
            function handleClearError(event){
                var formGroup = event.target.closest('.form-group');
                if(formGroup.classList.contains('invalid')){
                    formGroup.classList.remove('invalid');

                    var formMessage = formGroup.querySelector('.form-message');
                    if(formMessage){
                        formMessage.innerText = '';
                        
                    }
                }
            }
        }
    }

    // Xử lý hàm vi submit form 
    formElement.onsubmit = function(event){
        event.preventDefault();

        var inputs = formElement.querySelectorAll('[name][rule]');
        var isValid = true;
        for(var input of inputs){
            if(!handleValidate({target: input})){
                isValid = false;
            }
        }

        // khi không có lỗi thì submit form
        if(isValid){
            if(typeof _this.onSubmit == 'function'){

                var enabledInput = formElement.querySelectorAll('[name]:not([disabled])');
                    var formValues = Array.from(enabledInput).reduce(function(values,input){
                        switch(input.type){
                            case 'checkbox':
                                if(!input.matches(':checked')){
                                    return values;
                                }

                                if(!Array.isArray(values[input.name])){
                                    values[input.name] = [];
                                }

                                values[input.name].push(input.value);
                                break;
                            case 'radio':
                                values[input.name] = formElement.querySelector('input[name="' + input.name + '"]:checked').value;
                                
                              break;
                            default:
                             values[input.name] = input.value;
                        }
                        return values;
                    },{})
                    

                _this.onSubmit(formValues);
            }else{
                formElement.submit();             
            }
        }
    }

}