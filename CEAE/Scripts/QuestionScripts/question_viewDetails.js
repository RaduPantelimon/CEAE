$(document).ready(InitializePage);

//possibleAnswers
function InitializePage() {
    $("#new-answer").click(addNewAnswer);
}

function addNewAnswer() {
    var options = "";
    for (var i=0;i<possibleAnswers.length;i++)
    {
        var possibleAnswer = possibleAnswers[i];
        options += "<option value='" + possibleAnswer.AnswerID + "'>" + possibleAnswer.Text + "</option>"
    }
    $("#added-answers").append("<div class='answerContainer'></div>");

    //once the container was added, we add the remaining inputs
    addSecondaryElements($("#added-answers .answerContainer").last(),options);

}
function addSecondaryElements(newContainer, options) {

    $(newContainer).append("<select id='answer-select'></select>");
    var answersSelect = $(newContainer).find("select#answer-select").first();
    answersSelect.append(options);

    $(newContainer).append("<input id='rez1' val='' />");
    $(newContainer).append("<input id='rez2' val='' />");
    $(newContainer).append("<button class='deleteAnswer'>Delete Answer!</button>");

    //functionality to delete the answers added to this question
    var deleteButton = $(newContainer).find("button.deleteAnswer");
    deleteButton.click(function () {
        console.log("Stefania <3");
        $(this).closest(".answerContainer").remove();
    });
}