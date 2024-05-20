function LoadMoreContent(url, state, contentOutput) {
    if (state.cycle == null)
        state.cycle = 0;

    fetch(url + state.cycle)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.text(); // Assume response is HTML
        })
        .then(html => {
            const tempContainer = document.createElement('div');
            tempContainer.innerHTML = html;

            state.cycle++;
            contentOutput.appendChild(tempContainer);
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}