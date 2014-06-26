$(function () {

    $("#addContactToCampaign").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "http://localhost:1088/Contacts/SearchContacts",
                dataType: "json",
                data: {
                    featureClass: "P",
                    style: "full",
                    maxRows: 12,
                    searchString: request.term
                },
                success: function (data) {
                    response($.map(data, function (contact) {
                        return {
                            label: contact.Name + "    " + contact.PhoneNumber,
                            value: contact.Name,
                            ID: contact.ContactId,
                            Name: contact.Name,
                            Number: contact.PhoneNumber
                        }
                    }));

                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            console.log("event happened!");
            addToCampaignView(ui.item);
        }
    });
});

var addToCampaignView = function (contact) {
    console.log("ajaxin");
    console.log(contact)
    var campaignId = $('#CampaignId').attr('value');
    console.log("campaign id");
    console.log(campaignId);
    $.ajax({
        url: "http://localhost:1088/Campaigns/AddContact",
        type: "POST",
        data: {
            contactID: contact.ID,
            campaignID: campaignId
        },
        dataType: "json",
        success: function () {
            console.log("success")
            addToContactList(contact);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
            console.log(thrownError);
        }
    })

}

var addToContactList = function (contact) {
    prependToContactAttendees(contact);
    setUpDeleteButton(contact.ID);
    $("#campaign-attendees").scrollTop(0);
}

var setUpDeleteButton = function (contactID) {
    var contactRow = $("div").find("[contactID='" + contactID + "']");
    var deleteButton = contactRow[2];

    $(deleteButton).on('click', function () {

        removeFromCampaignDatabase(contactID, contactRow);
    });
}

var prependToContactAttendees = function (contact) {
    $("<div contactId='" + contact.ID + "'>").text(contact.Name).prependTo(".campaign-attendees-names");
    $("<div contactId='" + contact.ID + "'>").text(contact.Number).prependTo(".campaign-attendees-phonenumbers");
    $("<div class='delete-button-container' contactId='" + contact.ID + "'>").html("<button class='delete-button'>X</button>").prependTo(".campaign-attendees-delete");
}

var removeFromCampaignDatabase = function (contactID, contactRow) {
    var campaignID = $('#ID').attr('value');
    $.ajax({
        url: "http://localhost:1088/campaign/DeleteContact",
        type: "POST",
        data: {
            contactID: contactID,
            campaignID: campaignID
        },
        dataType: "json",
        success: function () {
            contactRow.remove();
        },

        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}