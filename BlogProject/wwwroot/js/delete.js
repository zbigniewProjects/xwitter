function MakeDeleteRequest(url, messageOutput) {
    $.ajax({
        url: url,
        type: 'DELETE',    
        contentType: 'application/json',
        dataType: "json",
        success: function (response, textStatus, xhr) {
            if (response.success) {
                window.location.href = response.redirectUrl;
            } else if (messageOutput != null) {
                messageOutput.innerHTML = response.errors;
            }
        },
        error: function (xhr, status, error) {
            console.error('Failed to delete resource:', error);
        }
    });
}
