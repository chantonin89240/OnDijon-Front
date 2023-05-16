function GetAbrisListe() {
    var select = document.getElementById("listeAbris");
    var option = document.createElement("option");
    select.appendChild(option);
    @if (Model.Abris != null && Model.Abris.Count > 0) {
        foreach(var abri in Model.Abris)
        {
            
            option.setAttribute('value', @abri.RecordId);
            select.appendChild(option);
            
        }

    }
}

