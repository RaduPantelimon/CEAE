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
        type: "GET",
        dataType: "json",
        success: SetAnswers,
        error: OnFail
    });
}

function SetAnswers(data) {
    console.log(data);

    //adding the data to the UI
    for (var i = 0; i < data.length; i++) {
        try {
            addNewAnswer();
            var lastAnswer = $("#added-answers .answerContainer").last();
            $(lastAnswer).find(".Value").val(data[i]["Value"]);
            $(lastAnswer).find(".Status").val(data[i]["Status"]);
            $(lastAnswer).find(".AnswerID").val(data[i]["AnswerID"]);
            $(lastAnswer).find(".AnswerID").attr("disabled", "true");
        } catch (ex) {
            console.log("Could not display element: " + ex);
        }


    }

}

function OnFail(jqXHR, exception) {
    console.log("Error while performing Ajax Call" + jqXHR + " " + exception);
}

function addNewAnswer(totheUI) {
    var options = "";
    for (var i = 0; i < possibleAnswers.length; i++) {
        var possibleAnswer = possibleAnswers[i];
        options += "<option value='" + possibleAnswer.AnswerID + "'>" + possibleAnswer.Text + "</option>";
    }
    $("#added-answers").append("<div class='row answerContainer'></div>");

    //once the container was added, we add the remaining inputs
    addSecondaryElements($("#added-answers .answerContainer").last(), options);

}

function addSecondaryElements(newContainer, options) {
    $(newContainer).append("");
    $(newContainer).append("<div class='col-sm-3'><select name='AnswerID' class='AnswerID form-control' id='answer-select'></select></div>");
    var answersSelect = $(newContainer).find("select#answer-select").first();
    answersSelect.append(options);
    $(newContainer).append("<div class='col-sm-3'><input name='Value'  class='Value form-control' val='' /></div>");
    //$(newContainer).append("<input name='Status' class='Status' val='' />");
    $(newContainer).append("<div class='col-sm-3'><select name='Status' class='Status form-control' val=''></select></div>");


    $(newContainer).append(
        "<div class='col-sm-3 text-left'>" +
            "<div class='btn-group'>" +
            "<button class='saveAnswer btn btn-primary' type='button'>Save</button>" +
            "<button class='deleteAnswer btn btn-danger' type='button'>Delete</button>" +
            "</div>" +
            "<img class='LoaderImage' src='/Content/Images/ajax-loader.gif' style='display:none;float:right;' height='20' width='20' />" +
        "</div>");

    var statusSelect = $(newContainer).find(".Status");
    if (answerStatuses && answerStatuses.length > 0) {
        for (var i = 0; i < answerStatuses.length; i++) {
            statusSelect.append("<option value='" + answerStatuses[i] + "'>" + answerStatuses[i] + "</option>");
        }
    }

    //functionality to delete the answers added to this question
    var deleteButton = $(newContainer).find("button.deleteAnswer");
    var saveButton = $(newContainer).find("button.saveAnswer");

    saveButton.click(function() {
        saveAnswer(newContainer);
    });

    deleteButton.click(function() {
        console.log("Stefania <3");
        DeleteAnswerQuestion(newContainer);

    });
}

function saveAnswer(newContainer) {
    //we retrieve the data
    var data = {};
    data.Value = $(newContainer).find(".Value").val();
    data.Status = $(newContainer).find(".Status").val();
    data.AnswerID = $(newContainer).find(".AnswerID").val();
    data.QuestionID = QuestionID;

    //we clear previous error, if existing
    $(".answers-validation").hide();
    $(".answers-validation-message").html("");

    //checking if there are any duplicate answers
    var timesPresent = 0;
    $(".answerContainer").each(function() {
        var newAnswerID = $(this).find(".AnswerID").val();
        if (newAnswerID == data.AnswerID) {
            timesPresent++;
        }
    });
    if (timesPresent > 1) {
        //the user is trying to add the same answer twice; stop him + error message
        $(".answers-validation").show();
        $(".answers-validation-message")
            .html("NU este posibila adaugarea de mai multe ori a aceluiasi raspuns la o singura intrebare");
        return;
    }

    //we initialize the Javascript Loader
    $(newContainer).find("img.LoaderImage").show();

    var jsonString = JSON.stringify(data);

    $.ajax({
        url: "/api/Rest/PostAnswerQuestion",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: jsonString,
        success: function(result) {
            if (!result.error) {
                //the item was just added // modified
                //we disable the answer addition
                $(newContainer).find(".AnswerID").attr("disabled", "true");

                //we deinitialize the Javascript Loader
                $(newContainer).find("img.LoaderImage").hide();
            }
        },
        error: function(jqXHR, exception) {

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

    var jsonString = JSON.stringify(data);

    $.ajax({
        url: "/api/Rest/DeleteAnswerQuestion",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: jsonString,
        success: function(result) {
            if (!result.error) {
                $(newContainer).find("img.LoaderImage").hide();
                $(newContainer).remove();
            }
        },
        error: function(jqXHR, exception) {
            OnFail(jqXHR, exception);
        }
    });
}