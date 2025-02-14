// leave it alone, does not work
export async function deleteFilm(apiBaseUrl, filmCopyId) {
    if (!filmCopyId) {
        console.error('Error: filmCopyId is undefined');
        alert('Error: filmCopyId is undefined');
        return;
    }

    if (confirm('Are you sure you want to delete this film?')) {
        try {
            const response = await fetch(`${apiBaseUrl}/film/${filmCopyId}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                }
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.error('Error deleting film:', errorData);
                throw new Error('Failed to delete film');
            }

            alert('Film deleted successfully');
            fetchAllFilms(apiBaseUrl); 
        } catch (error) {
            console.error('Error:', error);
            alert(`Error deleting film: ${error.message}`);
        }
    }
}

export function addFilm(apiBaseUrl) {
    const addFilmForm = document.getElementById('addFilmForm');

    if (addFilmForm) {
        addFilmForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const formData = new FormData(addFilmForm);
            const filmData = {
                title: formData.get('title'),
                description: formData.get('description'),
                availableCopies: parseInt(formData.get('availableCopies')),
                genre: formData.get('genre'),
                director: formData.get('director'),
                year: parseInt(formData.get('year')),
                isAvailable: formData.get('isAvailable') === 'on'
            };

            try {
                const response = await fetch(`${apiBaseUrl}/film`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    },
                    body: JSON.stringify(filmData)
                });

                if (!response.ok) {
                    if (response.status === 403) {
                        throw new Error('User does not have permission to add films. Please ensure you are logged in as an Admin.');
                    }
                    const errorData = await response.json();
                    console.error('Error adding film:', errorData);
                    throw new Error('Failed to add film');
                }

                alert('Film added successfully!');
                addFilmForm.reset();
                fetchAllFilms(apiBaseUrl);
            } catch (error) {
                console.error('Error:', error);
                alert(`Error adding film: ${error.message}`);
            }
        });
    }
}

export function fetchAllFilms(apiBaseUrl) {
    const filmList = document.getElementById('filmList');
    const isAdminPage = window.location.pathname.includes('admin.html');

    fetch(`${apiBaseUrl}/film`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to fetch films');
        }
        return response.json();
    })
    .then(films => {
        const filmsArray = films.$values || films;
        if (!Array.isArray(filmsArray)) {
            throw new Error('Films response is not an array');
        }

        filmList.innerHTML = '';
        if (filmsArray.length === 0) {
            filmList.innerHTML = '<p>No films available.</p>';
        } else {
            filmsArray.forEach(film => {
                const filmElement = document.createElement('div');
                filmElement.classList.add('film');
                filmElement.innerHTML = `
                    <h3>${film.title}</h3>
                    <p>${film.description}</p>
                    <p><strong>Director:</strong> ${film.director}</p>
                    <p><strong>Year:</strong> ${film.year}</p>
                    <p><strong>Genre:</strong> ${film.genre}</p>
                    <p><strong>Available Copies:</strong> ${film.availableCopies}</p>
                    ${isAdminPage ? `
                    <button class="delete-film-btn" data-film-id="${film.filmCopyId}">Delete</button>
                    <button onclick="editFilm(${film.filmCopyId})">Edit</button>
                    <form id="editFilmForm-${film.filmCopyId}" class="edit-film-form" style="display: none;">
                        <label for="title-${film.filmCopyId}">Title:</label>
                        <input type="text" id="title-${film.filmCopyId}" name="title" value="${film.title}" required>
                        <label for="description-${film.filmCopyId}">Description:</label>
                        <input type="text" id="description-${film.filmCopyId}" name="description" value="${film.description}" required>
                        <label for="availableCopies-${film.filmCopyId}">Available Copies:</label>
                        <input type="number" id="availableCopies-${film.filmCopyId}" name="availableCopies" value="${film.availableCopies}" required>
                        <label for="genre-${film.filmCopyId}">Genre:</label>
                        <input type="text" id="genre-${film.filmCopyId}" name="genre" value="${film.genre}" required>
                        <label for="director-${film.filmCopyId}">Director:</label>
                        <input type="text" id="director-${film.filmCopyId}" name="director" value="${film.director}" required>
                        <label for="year-${film.filmCopyId}">Year:</label>
                        <input type="number" id="year-${film.filmCopyId}" name="year" value="${film.year}" required>
                        <label for="isAvailable-${film.filmCopyId}">Is Available:</label>
                        <input type="checkbox" id="isAvailable-${film.filmCopyId}" name="isAvailable" ${film.isAvailable ? 'checked' : ''}>
                        <button type="submit">Save</button>
                    </form>` : ''}
                `;
                filmList.appendChild(filmElement);

                if (isAdminPage) {
                    document.getElementById(`editFilmForm-${film.filmCopyId}`).addEventListener('submit', async (event) => {
                        event.preventDefault();
                        const updatedFilmData = {
                            title: document.getElementById(`title-${film.filmCopyId}`).value,
                            description: document.getElementById(`description-${film.filmCopyId}`).value,
                            availableCopies: parseInt(document.getElementById(`availableCopies-${film.filmCopyId}`).value),
                            genre: document.getElementById(`genre-${film.filmCopyId}`).value,
                            director: document.getElementById(`director-${film.filmCopyId}`).value,
                            year: parseInt(document.getElementById(`year-${film.filmCopyId}`).value),
                            isAvailable: document.getElementById(`isAvailable-${film.filmCopyId}`).checked
                        };

                        try {
                            const response = await fetch(`${apiBaseUrl}/film/${film.filmCopyId}`, {
                                method: 'PATCH',
                                headers: {
                                    'Content-Type': 'application/json',
                                    'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                                },
                                body: JSON.stringify(updatedFilmData)
                            });

                            if (!response.ok) {
                                throw new Error('Failed to update film');
                            }

                            alert('Film updated successfully');
                            fetchAllFilms(apiBaseUrl);
                        } catch (error) {
                            console.error('Error:', error);
                            alert(`Error updating film: ${error.message}`);
                        }
                    });
                }
            });

            // Add event listeners for delete buttons
            document.querySelectorAll('.delete-film-btn').forEach(button => {
                button.addEventListener('click', (event) => {
                    const filmCopyId = event.target.getAttribute('data-film-id');
                    console.log(`Deleting film with ID: ${filmCopyId}`);
                    deleteFilm(apiBaseUrl, filmCopyId);
                });
            });
        }
    })
    .catch(error => {
        console.error('Error:', error);
        filmList.innerHTML = `<p>Error fetching films: ${error.message}</p>`;
    });
}
export async function editFilm(filmCopyId) {
    const editForm = document.getElementById(`editFilmForm-${filmCopyId}`);
    editForm.style.display = editForm.style.display === 'none' ? 'block' : 'none';
}

export function fetchAllFilmStudios(apiBaseUrl) {
    const getAllFilmStudiosButton = document.getElementById('getAllFilmStudiosButton');
    const filmStudioList = document.getElementById('filmStudioList');

    if (getAllFilmStudiosButton) {
        getAllFilmStudiosButton.addEventListener('click', async () => {
            try {
                const response = await fetch(`${apiBaseUrl}/filmstudio`, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch film studios');
                }

                const filmStudios = await response.json();
                const filmStudiosArray = filmStudios.$values || filmStudios;
                if (!Array.isArray(filmStudiosArray)) {
                    throw new Error('Film Studios response is not an array');
                }

                filmStudioList.innerHTML = '';

                if (filmStudiosArray.length === 0) {
                    filmStudioList.innerHTML = '<p>No film studios found.</p>';
                } else {
                    filmStudiosArray.forEach(studio => {
                        const studioElement = document.createElement('div');
                        studioElement.classList.add('studio');
                        studioElement.innerHTML = `
                            <p><strong>Username:</strong> ${studio.username}</p>
                            <p><strong>Name:</strong> ${studio.name}</p>
                            <p><strong>City:</strong> ${studio.city}</p>
                            <p><strong>Email:</strong> ${studio.email}</p>
                            <p><strong>Role:</strong> ${studio.role}</p>
                        `;
                        filmStudioList.appendChild(studioElement);
                    });
                }
            } catch (error) {
                console.error('Error:', error);
                filmStudioList.innerHTML = '<p>Error fetching film studios</p>';
            }
        });
    }
}

export function getFilmStudio(apiBaseUrl) {
    const getFilmStudioButton = document.getElementById('getFilmStudioButton');
    const filmStudioIdInput = document.getElementById('filmStudioIdInput');
    const filmStudioDetails = document.getElementById('filmStudioDetails');

    if (getFilmStudioButton) {
        getFilmStudioButton.addEventListener('click', async () => {
            const studioId = filmStudioIdInput.value;
            if (!studioId) {
                alert('Please enter a film studio ID');
                return;
            }

            try {
                const response = await fetch(`${apiBaseUrl}/filmstudio/${studioId}`, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch film studio');
                }

                const studio = await response.json();
                filmStudioDetails.innerHTML = `
                    <p><strong>Username:</strong> ${studio.username}</p>
                    <p><strong>Name:</strong> ${studio.name}</p>
                    <p><strong>City:</strong> ${studio.city}</p>
                    <p><strong>Email:</strong> ${studio.email}</p>
                    <p><strong>Role:</strong> ${studio.role}</p>
                `;
            } catch (error) {
                console.error('Error:', error);
                filmStudioDetails.innerHTML = '<p>Error fetching film studio</p>';
            }
        });
    }
}