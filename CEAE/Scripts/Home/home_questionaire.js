$(document).ready(InitializePage);

var questions = [];
var currentquestion;
var rezultat;

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
        $(".question-container").append("<div id='question-" + data[i].QuestionID + "' style='display:none;' class='question'><h2 class='sec-title-large'>" + data[i].Title + "</h2><hr class='sec-hr' /></div>");
       SetAnswers(data[i]);
    }
    

    //$('#question-1').show();
}

function SetAnswers(question) {
    //question-container
    //console.log(data);

    $('#question-' + question.QuestionID).append("<ul class='list row' ></ul>");
    var answersCntainer = $('#question-' + question.QuestionID + " ul");
    for (var i = 0; i < question.AnswersQuestions.length; i++) {
        answersCntainer.append("<li  id='answer-" + question.AnswersQuestions[i].Answer.AnswerID + "' class='answer sec-title-large col-lg-5 col-md-5 col-sm-12 col-xs-12'>" +
            "<input class=''  data-AnswerID='" + question.AnswersQuestions[i].Answer.AnswerID + "'  name='answer-" + question.QuestionID + "' type='radio'>" + question.AnswersQuestions[i].Answer.Text + "</li>");
    }
}


function StartTest() {
    NextQuestion();
    $("#start-questionaire").hide();
    $("#subm-button").click(SendAnswers);
    $("#prev-button").hide();
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
    ResolveButtonState();


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
    ResolveButtonState();
}

function ResolveButtonState() {

    if (currentquestion == questions.length - 1) {
        $("#subm-button").show();
        $("#prev-button").show();
        $("#next-button").hide();
    }
    else if (currentquestion == 0) {
        $("#subm-button").hide();
        $("#prev-button").hide();
        $("#next-button").show();
    }
    else {
        $("#subm-button").hide();
        $("#prev-button").show();
        $("#next-button").show();
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
                    //the item was just added // modified\
                   
                    console.log(result.raspusuriCorecte);
                    console.log(result);
                 
                    if (result.raspusuriCorecte > 0) {
                        
                        $('.quiz-status').html("Felicitari, ai avut " + result.raspusuriCorecte + " raspunsuri corecte!");
                    }
                    $('.question-container').hide();
                    $('.quiz-status').append("</br><img src='/Content/Images/40.jpg' " + " style='height='600' width='600' />");
                    $('.quiz-status').append("<br> <p> Știai că în România, peste 40% din elevii de 15 ani nu pot răspunde la aceste întrebări? Susține proiectele educaționale CEAE. Cu 10 lei un copil va învăța să gândească critic!</p>");
                  
                    $('.quiz-status').parent().show();
              


                }
            },
            error: function (jqXHR, exception) {

                OnFail(jqXHR, exception);
            }
        });

    
        
    }
}