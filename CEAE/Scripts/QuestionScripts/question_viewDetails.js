$(document).ready(InitializePage);

//possibleAnswers
function InitializePage() {
    $("#new-answer").click(addNewAnswer);
    getExistingAnswers();
}

function getExistingAnswers() {
    $.ajax({
        url: "/api/Rest/GetAnswersQuestions/" + QuestionID,
        contentType: "application/json; charset=utf-8",
        type: 'GET',
        dataType: "json",
        success: SetAnswers,
        error: OnFail
    });
}

function SetAnswers(data) {
    console.log(data);

    //adding the data to the UI
    for(var i=0;i<data.length;i++)
    {
        try{
            addNewAnswer();
            var lastAnswer = $("#added-answers .answerContainer").last();
            $(lastAnswer).find(".Value").val(data[i]["Value"]);
            $(lastAnswer).find(".Status").val(data[i]["Status"]);
            $(lastAnswer).find(".AnswerID").val(data[i]["AnswerID"]);
            $(lastAnswer).find(".AnswerID").attr("disabled", "true");
        }
        catch(ex)
        {
            console.log("Could not display element: " + ex);
        }
       

    }

}
function OnFail(jqXHR, exception) {
    console.log("Error while performing Ajax Call" + jqXHR + " " + exception);
}

function addNewAnswer(totheUI) {
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
    $(newContainer).append("<img class='LoaderImage' src='/Content/Images/ajax-loader.gif' " +
        " style='display:none;' height='20' width='20' />")

    $(newContainer).append("<select name='AnswerID' class='AnswerID' id='answer-select'></select>");
    var answersSelect = $(newContainer).find("select#answer-select").first();
    answersSelect.append(options);
    $(newContainer).append("<input name='Value'  class='Value' val='' />");
    //$(newContainer).append("<input name='Status' class='Status' val='' />");
    $(newContainer).append("<select name='Status' class='Status' val=''></select>");


    $(newContainer).append("<button class='saveAnswer'  type='button' >Save changes</button>");
    $(newContainer).append("<button class='deleteAnswer'  type='button' >Delete Answer!</button>");


    var statusSelect = $(newContainer).find(".Status");
    if (answerStatuses && answerStatuses.length > 0)
    {
        for (var i = 0; i < answerStatuses.length;i++)
        {
            statusSelect.append("<option value='" + answerStatuses[i] + "'>" + answerStatuses[i] + "</option>");
        }
    }

    //functionality to delete the answers added to this question
    var deleteButton = $(newContainer).find("button.deleteAnswer");
    var saveButton = $(newContainer).find("button.saveAnswer");

    saveButton.click(function () {
        saveAnswer(newContainer);
    });

    deleteButton.click(function () {
        console.log("Stefania <3");
        DeleteAnswerQuestion(newContainer);

    });
}

function saveAnswer(newContainer)
{
    //we retrieve the data
    var data = {};
    data.Value = $(newContainer).find(".Value").val();
    data.Status = $(newContainer).find(".Status").val();
    data.AnswerID = $(newContainer).find(".AnswerID").val();
    data.QuestionID = QuestionID;

    //we clear previous error, if existing
    $(".answers-validation-message").html("");

    //checking if there are any duplicate answers
    var timesPresent = 0;
    $(".answerContainer").each(function () {
        var newAnswerID= $(this).find(".AnswerID").val();
        if (newAnswerID == data.AnswerID) {
            timesPresent++;
        }
    })
    if (timesPresent > 1)
    {
        //the user is trying to add the same answer twice; stop him + error message
        $(".answers-validation-message").html("NU este posibila adaugarea de mai multe ori a aceluiasi raspuns la o singura intrebare");
        return;
    }
    
    //we initialize the Javascript Loader
    $(newContainer).find("img.LoaderImage").show();
    $(newContainer).css("padding-left", "0px");

    var jsonString = JSON.stringify(data);

    $.ajax({
        url: "/api/Rest/PostAnswerQuestion",
        contentType: "application/json; charset=utf-8",
        type: 'POST' ,
        dataType: "json",
        data: jsonString,
        success: function (result) {
            if (!result.error) {
                //the item was just added // modified
                //we disable the answer addition
                $(newContainer).find(".AnswerID").attr("disabled", "true");

                //we deinitialize the Javascript Loader
                $(newContainer).find("img.LoaderImage").hide();
                $(newContainer).css("padding-left", "20px");
            }
        },
        error: function (jqXHR, exception)
        {

            OnFail(jqXHR, exception);
        }
    });
}
function DeleteAnswerQuestion(newContainer) {
    var data = {};
    data.Value = $(newContainer).find(".Value").val();
    data.Status = $(newContainer).find(".Status").val();
    data.AnswerID = $(newContainer).find(".AnswerID").val();
    data.QuestionID = QuestionID;


    //we initialize the Javascript Loader
    $(newContainer).find("img.LoaderImage").show();
    $(newContainer).css("padding-left", "0px");

    var jsonString = JSON.stringify(data);

    $.ajax({
        url: "/api/Rest/DeleteAnswerQuestion",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        dataType: "json",
        data: jsonString,
        success: function (result) {
            if (!result.error) {
                $(newContainer).find("img.LoaderImage").hide();
                $(newContainer).css("padding-left", "20px");
                $(newContainer).remove();
            }
        },
        error: function (jqXHR, exception)
        {
            OnFail(jqXHR, exception);
        }
    });
}