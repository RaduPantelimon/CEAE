﻿$(document).ready(InitializePage);

//possibleAnswers
function InitializePage() {
    console.log("jeg");
    $("#new-answer").click(addNewAnswer);
    getExistingAnswers();
}

function getExistingAnswers() {
    $.ajax({
        url: "/api/Rest/GetAnswersQuestions/1",
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

    $(newContainer).append("<select name='AnswerID' class='AnswerID' id='answer-select'></select>");
    var answersSelect = $(newContainer).find("select#answer-select").first();
    answersSelect.append(options);

    $(newContainer).append("<input name='Value'  class='Value' val='' />");
    $(newContainer).append("<input name='Status' class='Status' val='' />");
    $(newContainer).append("<button class='saveAnswer'>Save changes</button>");
    $(newContainer).append("<button class='deleteAnswer'>Delete Answer!</button>");

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

    var data = {};
    data.Value = $(newContainer).find(".Value").val();
    data.Status = $(newContainer).find(".Status").val();
    data.AnswerID = $(newContainer).find(".AnswerID").val();
    data.QuestionID = QuestionID;

    var jsonString = JSON.stringify(data);

    $.ajax({
        url: "/api/Rest/PostAnswerQuestion",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        dataType: "json",
        data: jsonString,
        success: function (result) {
            if (!result.error) {
                //the item was just added // modified
                //we disable the answer addition
                $(newContainer).find(".AnswerID").attr("disabled", "true");
            }
        },
        error: OnFail
    });
}
function DeleteAnswerQuestion(newContainer) {
    var data = {};
    data.Value = $(newContainer).find(".Value").val();
    data.Status = $(newContainer).find(".Status").val();
    data.AnswerID = $(newContainer).find(".AnswerID").val();
    data.QuestionID = QuestionID;

    var jsonString = JSON.stringify(data);

    $.ajax({
        url: "/api/Rest/DeleteAnswerQuestion",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        dataType: "json",
        data: jsonString,
        success: function (result) {
            if (!result.error) {

                $(newContainer).remove();
            }
        },
        error: OnFail
    });
}