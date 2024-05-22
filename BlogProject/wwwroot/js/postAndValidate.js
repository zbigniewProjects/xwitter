function PostAndValidateAndRedirect(url, data, errorOutput) {
    let jsonData = JSON.stringify(data);

    console.log(jsonData);

    $.ajax({
        url: url,
        type: 'POST',
        data: jsonData,
        cache: false,
        processData: false,
        contentType: 'application/json',
        success: function (response, textStatus, xhr) {
            //console.log(xhr.status);
            if (response.success) {
                window.location.href = response.redirectUrl;
            } else if (errorOutput != null) {
                errorOutput.innerHTML = '';
                var errorMessages = response.errors.join('\n');
                errorOutput.innerHTML = errorMessages;
                console.log(errorMessages);
            }
        },
        error: function (xhr, status, error) {
            console.error(status);
            errorOutput.innerHTML = 'Cannot connect to zbigniew.dev, check Your internet connection';
            history.replaceState(null, null, window.location.href);
        }
    });
}
