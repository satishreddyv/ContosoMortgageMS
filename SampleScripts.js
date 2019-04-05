// JavaScript source code
function ContactLoad(executionContext) {
    var formContext = executionContext.getFormContext();

    alert("Hello " + formContext.getAttribute("lastname").getValue());
}


function ContactSave(executionContext) {
    var formContext = executionContext.getFormContext();


    alert("This is Save event");


}


function ContactEmailChange(executionContext) {
    var formContext = executionContext.getFormContext();
    alert("This is On change of Email");
}
