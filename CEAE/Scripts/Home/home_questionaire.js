$(document).ready(InitializePage);

var questions = [];
var currentquestion;

function InitializePage() {

    getQuestions();


}

function getQuestions() {
    $.ajax({
        url: "/Questionnaire/GetQuestions",
        contentType: "application/json; charset=utf-8",
        type: 'GET',
        dataType: "json",
        success: SetData,
        error: OnFail
    });
}
function SetData(data){
    //set start event
    SetQuestions(data);
    $("#start-questionaire").click(StartTest);
    $("#next-button").click(NextQuestion);
    $("#prev-button").click(PrevQuestion);


}
function OnFail(jqXHR, exception) {
    console.log("Error while performing Ajax Call" + jqXHR + " " + exception);
}

function SetQuestions(data) {
    //question-container
    //console.log(data);
    questions = data;
    for(var i=0;i<data.length;i++)
    {
        $(".question-container").append("<div id='question-"+ data[i].QuestionID + "' style='display:none;' class='question'>" + data[i].Title + "</div>");
       SetAnswers(data[i]);
    }
    

    //$('#question-1').show();
}

function SetAnswers(question) {
    //question-container
    //console.log(data);

    $('#question-' + question.QuestionID).append("<ul class='list' ></ul>");
    var answersCntainer = $('#question-' + question.QuestionID + " ul");
    for (var i = 0; i < question.AnswersQuestions.length; i++) {
        answersCntainer.append("<li  id='answer-" + question.AnswersQuestions[i].Answer.AnswerID + "' class='answer'>" +
            "<input  data-AnswerID='" + question.AnswersQuestions[i].Answer.AnswerID + "'  name='answer-" + question.QuestionID + "' type='radio'>" + question.AnswersQuestions[i].Answer.Text + "</li>");
    }
}


function StartTest() {
    NextQuestion();
    $("#start-questionaire").hide();
    $("#prev-button").show();
    $("#next-button").show();

}

function NextQuestion() {
    if (!currentquestion && currentquestion !== 0) {
        currentquestion = 0;
    }
    else if (currentquestion < questions.length-1) {
        currentquestion++;
    }
    if (questions.length && questions.length >= currentquestion) {
        $('.question').hide();
        $('#question-' + questions[currentquestion].QuestionID).show();

        //     for (var i = 0; questions[currentquestion].AnswersQuestions.length ; i++)
        //         $('#answer-' + questions[currentquestion].AnswersQuestions[i].Answer.Text).show();


      


    }
    if (currentquestion == questions.length-1) {
        $("#subm-button").show();
        $("#prev-button").hide();
        $("#next-button").hide();
        $("#subm-button").click(SendAnswers);


    }

}
function PrevQuestion() {
    if (!currentquestion && currentquestion !== 0) {
        currentquestion = 0;
    }
    else {
        currentquestion--;
    }
    if (questions.length && currentquestion >= 0) {
        $('.question').hide();
        $('#question-' + questions[currentquestion].QuestionID).show();

    }
}



function SendAnswers() {
    if (questions.length)
    {
        var solutions = [];
        for(var i=0;i<questions.length;i++)
        {
            var answerID = 0;
            var selectedAnswer = $("input[name='answer-" + questions[i].QuestionID + "']:checked");
            if(selectedAnswer && selectedAnswer.length>0)
            {
                answerID = selectedAnswer.attr('data-AnswerID');
            }

            //procesam in partea de serverside, folosind Modelul QuestionnaireAnswer
            //public int QuestionID;
            //public int AnswerID;
            var selectedResponse = {};
            selectedResponse.QuestionID = questions[i].QuestionID;
            selectedResponse.AnswerID = answerID;
            solutions.push(selectedResponse);



        }

        var jsonString = JSON.stringify(solutions);
        $.ajax({
            url: "/Questionnaire/SetAnswers",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            dataType: "json",
            data: jsonString,
            success: function (result) {
                if (!result.error) {
                    //the item was just added // modified
                    console.log(result.raspusuriCorecte);
                    console.log(result);
                    
                }
            },
            error: function (jqXHR, exception) {

                OnFail(jqXHR, exception);
            }
        });
    }
}