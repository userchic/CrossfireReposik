// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .build();

hubConnection.on('EndGameMessage', () =>
{
    window.location.href = '/Gamer/Results'
    //переход на страницу с результатами
})
hubConnection.on('SolvedTaskMessage', (taskId, isSuccessful, IsShotSuccessful) =>
{
    document.getElementById("btn." + taskId).setAttribute('disabled', true);
    document.getElementById("answer." + taskId).setAttribute('disabled', true);
    document.getElementById("TargetTeam." + taskId).setAttribute('disabled', true);
    var text = document.getElementById("line." + taskId)
    text.innerHTML += "<p>"
    if (isSuccessful) {
        text.innerHTML += "Правильно решено"
        if (IsShotSuccessful)
            text.InnerHTML += "Выстрел успешен"
        else
            text.InnerHTML += "Выстрел безуспешен"
    }
    else {
        text.innerHTML +="Неправильно решено"
    }
    text.innerHtml +="</p>"
})
hubConnection.start().then(() =>
{
    teamId = document.getElementById("gameID").value;
    hubConnection.invoke("ConnectToTheGame", Number(teamId));
});



