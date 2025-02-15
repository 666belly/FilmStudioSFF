import { rentFilmToStudio } from './filmStudioManagement.js';

export async function deleteFilm(apiBaseUrl, filmId) {
    if (!filmId) {
        console.error('Error: filmId is undefined');
        alert('Error: filmId is undefined');
        return;
    }

    if (confirm('Are you sure you want to delete this film?')) {
        try {
            const response = await fetch(`${apiBaseUrl}/film/${filmId}`, {
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
    const filmStudioId = localStorage.getItem('filmStudioId'); // Get the filmStudioId from localStorage
    const isFilmStudioPage = window.location.pathname.includes('filmstudio.html');
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
                `;

                if (isFilmStudioPage) {
                    film.filmCopies.forEach(copy => {
                        if (!copy.isRented) {
                            const rentButton = document.createElement('button');
                            rentButton.textContent = `Rent ${copy.title}`;
                            rentButton.classList.add('rent-film-btn');
                            rentButton.setAttribute('data-film-id', copy.filmCopyId);
                            rentButton.addEventListener('click', () => {
                                rentFilmToStudio(apiBaseUrl, filmStudioId, copy.filmCopyId);
                            });

                            filmElement.appendChild(rentButton);
                        }
                    });
                }

                if (isAdminPage) {
                    const deleteButton = document.createElement('button');
                    deleteButton.textContent = 'Delete';
                    deleteButton.classList.add('delete-film-btn');
                    deleteButton.setAttribute('data-film-id', film.filmId);
                    deleteButton.addEventListener('click', (event) => {
                        const filmId = event.currentTarget.getAttribute('data-film-id');
                        deleteFilm(apiBaseUrl, filmId);
                    });

                    const editButton = document.createElement('button');
                    editButton.textContent = 'Edit';
                    editButton.addEventListener('click', () => {
                        editFilm(film.filmId);
                    });

                    filmElement.appendChild(deleteButton);
                    filmElement.appendChild(editButton);

                    const editForm = document.createElement('form');
                    editForm.id = `editFilmForm-${film.filmId}`;
                    editForm.classList.add('edit-film-form');
                    editForm.style.display = 'none';
                    editForm.innerHTML = `
                        <label for="title-${film.filmId}">Title:</label>
                        <input type="text" id="title-${film.filmId}" name="title" value="${film.title}" required>
                        <label for="description-${film.filmId}">Description:</label>
                        <input type="text" id="description-${film.filmId}" name="description" value="${film.description}" required>
                        <label for="availableCopies-${film.filmId}">Available Copies:</label>
                        <input type="number" id="availableCopies-${film.filmId}" name="availableCopies" value="${film.availableCopies}" required>
                        <label for="genre-${film.filmId}">Genre:</label>
                        <input type="text" id="genre-${film.filmId}" name="genre" value="${film.genre}" required>
                        <label for="director-${film.filmId}">Director:</label>
                        <input type="text" id="director-${film.filmId}" name="director" value="${film.director}" required>
                        <label for="year-${film.filmId}">Year:</label>
                        <input type="number" id="year-${film.filmId}" name="year" value="${film.year}" required>
                        <label for="isAvailable-${film.filmId}">Is Available:</label>
                        <input type="checkbox" id="isAvailable-${film.filmId}" name="isAvailable" ${film.isAvailable ? 'checked' : ''}>
                        <button type="submit">Save</button>
                    `;

                    editForm.addEventListener('submit', async (event) => {
                        event.preventDefault();
                        const updatedFilmData = {
                            filmId: film.filmId,
                            title: document.getElementById(`title-${film.filmId}`).value,
                            description: document.getElementById(`description-${film.filmId}`).value,
                            availableCopies: parseInt(document.getElementById(`availableCopies-${film.filmId}`).value),
                            genre: document.getElementById(`genre-${film.filmId}`).value,
                            director: document.getElementById(`director-${film.filmId}`).value,
                            year: parseInt(document.getElementById(`year-${film.filmId}`).value),
                            isAvailable: document.getElementById(`isAvailable-${film.filmId}`).checked
                        };

                        try {
                            const response = await fetch(`${apiBaseUrl}/film/${film.filmId}`, {
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

                    filmElement.appendChild(editForm);
                }

                filmList.appendChild(filmElement);
            });
        }
    })
    .catch(error => {
        console.error('Error:', error);
        filmList.innerHTML = `<p>Error fetching films: ${error.message}</p>`;
    });
}

export async function editFilm(filmId) {
    const editForm = document.getElementById(`editFilmForm-${filmId}`);
    if (editForm) {
        editForm.style.display = editForm.style.display === 'none' ? 'block' : 'none';
    } else {
        console.error(`Edit form for filmId ${filmId} not found`);
    }
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