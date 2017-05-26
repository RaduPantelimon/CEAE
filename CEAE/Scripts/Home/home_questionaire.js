$(function () {

    var isLoggedIn = window["isLoggedIn"] || false;
    var registeredEmail = window["registeredEmail"] || false;

    var disabledCallback = {};
    var questions = [];
    var currentquestion;

    function buttonHide($button) {
        $button.attr("disabled", true);
        disabledCallback[$button.attr("id")] = true;
    }

    function buttonShow($button) {
        $button.removeAttr("disabled");
        disabledCallback[$button.attr("id")] = false;
    }

    function handleButtonPrevNext($prev, $next, hasPrev, hasNext) {
        if (hasPrev) {
            buttonShow($prev);
        } else {
            buttonHide($prev);
        }

        if (hasNext) {
            buttonShow($next);
        } else {
            buttonHide($next);
        }
    }

    function buttonHandleClick($button, trueCallback) {
        $button.click(function () {
            if (disabledCallback[$button.attr("id")])
                return;
            trueCallback();
        });
    }

    function getJson(url, success, error) {
        $.ajax({
            url: url,
            contentType: "application/json; charset=utf-8",
            type: "GET",
            dataType: "json",
            success: success,
            error: error
        });
    }

    function getQuestions() {
        getJson("/Questionnaire/GetQuestions", setData, onFail);
    }

    function setData(data) {
        var $sq = $("#start-questionaire");
        var $p = $("#prev-button");
        var $n = $("#next-button");
        var $l = $("#loading");

        // hide loading screen
        $l.hide(233);

        //set start event
        setQuestions(data);

        $sq.click(startTest);
        $sq.removeAttr("disabled");

        // hide all elements first
        
        $p.hide();
        $n.hide();

        buttonHandleClick($n, nextQuestion);
        buttonHandleClick($p, prevQuestion);
    }

    function onFail(jqXhr, exception) {
        console.error("Ajax call fail:", jqXhr, exception);
    }

    function setQuestions(data) {
        // #question-container
        console.log("SetQuestions", data);

        questions = data;

        var $qc = $("#question-container");
        var imageStr;

        for (var i = 0; i < data.length; i++) {
            imageStr = data[i].Text
                ? "<img src='/Content/Images/" + data[i].Text + "'" + " style='height='300px'/>"
                : "";
            $qc.append("<div id='question-" +
                data[i].QuestionID +
                "' style='display:none;' class='question'><h2 class='sec-title-large'>" +
                data[i].Title +
                "</h2><hr class='sec-hr' />" +
                imageStr +
                "</div>" +
                "</br>"
            );
            setAnswers(data[i]);
        }


        //$('#question-1').show();
    }

    function setAnswers(question) {
        //question-container
        console.log("SetAnswers", question);

        $("#question-" + question.QuestionID).append("<ul class='list row' ></ul>");
        var answersCntainer = $("#question-" + question.QuestionID + " ul");
        for (var i = 0; i < question.AnswersQuestions.length; i++) {
            answersCntainer.append("<li  id='answer-" +
                question.AnswersQuestions[i].Answer.AnswerID +
                "' class='answer sec-title-large col-lg-5 col-md-5 col-sm-12 col-xs-12'>" +
                "<input class=''  data-AnswerID='" +
                question.AnswersQuestions[i].Answer.AnswerID +
                "'  name='answer-" +
                question.QuestionID +
                "' type='radio'>" +
                question.AnswersQuestions[i].Answer.Text +
                "</li>");
        }
    }


    function startTest() {
        nextQuestion();
        $("#start-questionaire").hide();
        $("#subm-button").click(checkUserStatus);

        var $p = $("#prev-button");
        var $n = $("#next-button");

        $p.show();
        $n.show();
        handleButtonPrevNext($p, $n, false, true);
    }

    function checkUserStatus() {
        var $p = $("#prev-button");
        var $n = $("#next-button");
        var $m = $(this);

        handleButtonPrevNext($p, $n, false, false); 
        $m.attr("disabled", true);

        if (isLoggedIn || registeredEmail) {
            sendAnswers();
        } else {
            $("#question-section").hide();
            $(".email-container").show();

            $("#subm-email-button").click(submitEmail);
        }

    }

    function nextQuestion() {
        if (!currentquestion && currentquestion !== 0) {
            currentquestion = 0;
        } else if (currentquestion < questions.length - 1) {
            currentquestion++;
        }
        if (questions.length && questions.length >= currentquestion) {
            $(".question").hide();
            $("#question-" + questions[currentquestion].QuestionID).show();

            //     for (var i = 0; questions[currentquestion].AnswersQuestions.length ; i++)
            //         $('#answer-' + questions[currentquestion].AnswersQuestions[i].Answer.Text).show();
        }
        resolveButtonState();


    }

    function validateEmail(email) {
        return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            .test(email);
    }


    function setErrorEmail(hasError) {
        var $errorLabel = $("#email-validation");
        var $formGroup = $errorLabel.closest(".form-group");
        if (hasError) {
            $errorLabel.show();
            $formGroup.addClass("has-error");
        } else {
            $errorLabel.hide();
            $formGroup.removeClass("has-error");
        }
    }

    function submitEmail() {
        var $btn = $(this);
        $btn.attr("disabled", true);
        console.log("submit email", this);
        var emailAddress = $("#email-input").val();
        var dataToSend = {};
        dataToSend.emailAddress = emailAddress;
        var jsonString = JSON.stringify(dataToSend);

        var hasError = !validateEmail(emailAddress);
        console.log("has error submitting email?: ", hasError);
        setErrorEmail(hasError);


        if (hasError) {
            $btn.removeAttr("disabled");
            return;
        }

        $.ajax({
            url: "/Questionnaire/SetEmail",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            dataType: "json",
            data: jsonString,
            success: function (result) {
                $btn.removeAttr("disabled");

                if (result.status) {
                    // now user has a Contact model.
                    $("#question-section").show();
                    $(".email-container").hide();
                    window.registeredEmail = true;
                    sendAnswers();
                } else {
                    console.log("received error", result);
                    //to do mesaj de eroare
                }
            },
            error: function (jqXhr, exception) {
                $btn.removeAttr("disabled");
                onFail(jqXhr, exception);
            }
        });
    }

    function prevQuestion() {
        if (!currentquestion && currentquestion !== 0) {
            currentquestion = 0;
        } else {
            currentquestion--;
        }
        if (questions.length && currentquestion >= 0) {
            $(".question").hide();
            $("#question-" + questions[currentquestion].QuestionID).show();
        }
        resolveButtonState();
    }

    function resolveButtonState() {
        var showPrev, showNext, showSubmit = false;

        var $p = $("#prev-button");
        var $n = $("#next-button");

        if (currentquestion === questions.length - 1) {
            showSubmit = true;
            showPrev = true;
            showNext = false;
        } else if (currentquestion === 0) {
            showPrev = false;
            showNext = true;
        } else {
            showPrev = true;
            showNext = true;
        }

        handleButtonPrevNext($p, $n, showPrev, showNext);

        if (showSubmit) {
            $("#subm-button").css("display", "block");
        } else {
            $("#subm-button").hide();
        }

    }

    function getSolutions() {
        var solutions = [];
        for (var i = 0; i < questions.length; i++) {
            var answerID = 0;
            var $answer = $("input[name='answer-" + questions[i].QuestionID + "']:checked");

            if ($answer && $answer.length > 0) {
                answerID = $answer.attr("data-AnswerID");
            }

            //procesam in partea de serverside, folosind Modelul QuestionnaireAnswer
            //public int QuestionID;
            //public int AnswerID;
            var selectedResponse = {
                QuestionID: questions[i].QuestionID,
                AnswerID: answerID
            };
            solutions.push(selectedResponse);


        }

        return solutions;
    }

    function sendAnswers() {
        var $m = $("#subm-button");

        if (!questions.length)
            return;

        var jsonString = JSON.stringify(getSolutions());

        $.ajax({
            url: "/Questionnaire/SetAnswers",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            dataType: "json",
            data: jsonString,
            success: function (result) {
                $m.removeAttr("disabled");
                    
                if (!result.error) {

                    result = result.message;

                    console.log(result);

                    $("#quiz-status").html(result.header);

                    $("#question-container").hide();
                    $("#subm-button").hide();
                    $("#prev-button").hide();
                    $("#next-button").hide();

                    $("#quiz-status").parent().removeClass("hidden");


                }
            },
            error: function(jqXhr, exception) {
                $("#subm-button").removeAttr("disabled");
                onFail(jqXhr, exception);
            }
        });

    }

    // CONSTRUCTOR

    getQuestions();
});