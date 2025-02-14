import { registerUser, loginUser, fetchAllUsers, getUser } from './modules/userManagement.js';
import { addFilm, fetchAllFilms as fetchAllFilmsAdmin, editFilm, deleteFilm, fetchAllFilmStudios, getFilmStudio } from './modules/filmManagement.js';
import { registerFilmStudio, loginFilmStudio, fetchRentedFilms, fetchAllFilms as fetchAllFilmsStudio, rentFilm, returnFilm } from './modules/filmStudioManagement.js';
import { logout } from './modules/utility.js';

const apiBaseUrl = 'http://localhost:5145/api';

document.addEventListener('DOMContentLoaded', () => {
    const isAdminPage = window.location.pathname.includes('admin.html');
    const isFilmStudioPage = window.location.pathname.includes('filmstudio.html');
    const isRegisterPage = window.location.pathname.includes('register.html');
    const isLoginPage = window.location.pathname.includes('login.html');

    if (isRegisterPage) {
        registerUser(apiBaseUrl);
        registerFilmStudio(apiBaseUrl);
    }

    if (isLoginPage) {
        loginUser(apiBaseUrl);
        loginFilmStudio(apiBaseUrl);
    }

    if (isAdminPage) {
        addFilm(apiBaseUrl);
        fetchAllFilmsAdmin(apiBaseUrl);
        fetchAllUsers(apiBaseUrl);
        getUser(apiBaseUrl);
        fetchAllFilmStudios(apiBaseUrl);
        getFilmStudio(apiBaseUrl);

        window.editFilm = editFilm;
        window.deleteFilm = (filmCopyId) => {
            console.log(`Deleting film with ID: ${filmCopyId}`); // Debugging
            deleteFilm(apiBaseUrl, filmCopyId);
        };

        const toggleAddFilmFormButton = document.getElementById('toggleAddFilmForm');
        if (toggleAddFilmFormButton) {
            toggleAddFilmFormButton.addEventListener('click', () => {
                const addFilmForm = document.getElementById('addFilmForm');
                addFilmForm.style.display = addFilmForm.style.display === 'none' ? 'block' : 'none';
            });
        }

        const logoutButton = document.getElementById('logoutButton');
        if (logoutButton) {
            logoutButton.addEventListener('click', logout);
        }
    }

    if (isFilmStudioPage) {
        fetchRentedFilms(apiBaseUrl);
        fetchAllFilmsStudio(apiBaseUrl);
        returnFilm(apiBaseUrl);

        window.rentFilm = (filmId) => {
            rentFilm(filmId);
        };
    }
});