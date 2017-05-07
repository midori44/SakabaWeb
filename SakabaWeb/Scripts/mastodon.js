$(function () {
    function AddInfo() {
        $("#sakaba").remove();
        $(".compose-form").after(
            $("<div id='sakaba'><div class='sakaba-info'><h3>酒場からのお知らせ</h3>BOSS召喚コマンド<br /><a href='http://sakaba-boss.azurewebsites.net/'>http://sakaba-boss.azurewebsites.net/</a><br />user: sakaba / pass: sakaba2017</div></div>")
        );
    }

    AddInfo();
    $(".tabs-bar__link").click(function () {
        setTimeout(function () {
            AddInfo();
        }, 1000);
    });
});
