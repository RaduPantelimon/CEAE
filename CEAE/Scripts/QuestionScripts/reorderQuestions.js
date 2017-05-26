$(document).ready(InitializePage);

function InitializePage()
{
    //initialize jquery UI sorting behaviour
    InitializeSorting();

    //initialize data submission
    $(".submit-values").click(DataSubmission);
}

function InitializeSorting() {
    $("#sortable").sortable();
    $("#sortable").disableSelection();
}

function DataSubmission()
{
    //parsing the data that we'll send to the server
    var questionsToOrder = [];
    var qsIndex = 1;
    var updateSolution = false;
    $(".orderable-questions .ordered-question").each(function () {
        var question = $(this);

        var originalOrder = question.attr("originalOrder");
        var questionID = question.attr("questionID");

        if(originalOrder != qsIndex)
        {
            //solution is worth updating
            var questionUpdate = {};
            questionUpdate.QuestionOrder = qsIndex;
            questionUpdate.QuestionID = questionID;

            questionsToOrder.push(questionUpdate);
            updateSolution = true;
        }
        qsIndex++;
    })

    var jsonString = JSON.stringify(questionsToOrder);

    if (updateSolution)
    {
        //we'll send the results
        $.ajax({
            url: "/Questions/SaveOrder",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            dataType: "json",
            data: jsonString,
            success: function (result) {
                if (!result.error) {
                    //the item was just added // modified
                    //we disable the answer addition
                    $(newContainer).find(".AnswerID").attr("disabled", "true");

                    //we deinitialize the Javascript Loader
                    $(newContainer).find("img.LoaderImage").hide();
                }
            },
            error: function (jqXHR, exception) {

                OnFail(jqXHR, exception);
            }
        });
    }

}