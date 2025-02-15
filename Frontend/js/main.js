import { fetchAllFilms } from './modules/filmManagement.js';
import { registerUser, loginUser, fetchAllUsers, getUser } from './modules/userManagement.js';
import { registerFilmStudio, loginFilmStudio, fetchRentedFilms, rentFilmToStudio } from './modules/filmStudioManagement.js';
import { addFilm, fetchAllFilms as fetchAllFilmsAdmin, editFilm, deleteFilm, fetchAllFilmStudios, getFilmStudio } from './modules/filmManagement.js';
import { logout } from './modules/utility.js';


document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = 'http://localhost:5145/api';

    const isHomePage = window.location.pathname.includes('index.html');
    const isAdminPage = window.location.pathname.includes('admin.html');
    const isRegisterPage = window.location.pathname.includes('register.html');
    const isLoginPage = window.location.pathname.includes('login.html');
    const isFilmStudioPage = window.location.pathname.includes('filmstudio.html');

    if (isHomePage) {
        fetchAllFilms(apiBaseUrl);
    }

    if (isRegisterPage) {
        registerUser(apiBaseUrl);
        registerFilmStudio(apiBaseUrl);
    }
    if (isFilmStudioPage) {
        const filmStudioId = localStorage.getItem('filmStudioId'); 
        console.log('Retrieved filmStudioId from localStorage:', filmStudioId); 
    
        if (!filmStudioId) {
            console.error('No filmStudioId found in local storage. Redirecting to login...');
            window.location.href = 'login.html'; 
            return;
        }
    
        fetchAllFilms(apiBaseUrl); 
        fetchRentedFilms(apiBaseUrl, filmStudioId); 
    
        const rentFilmForm = document.getElementById('rentFilmForm');
        if (rentFilmForm) {
            rentFilmForm.addEventListener('submit', (event) => {
                event.preventDefault();
                const filmId = document.getElementById('filmId').value;
                if (!filmId) {
                    console.error('No filmId provided');
                    return;
                }
                rentFilmToStudio(apiBaseUrl, filmStudioId, filmId); 
            });
        }

        const logoutButton = document.getElementById('logoutButton');
        if (logoutButton) {
            logoutButton.addEventListener('click', logout);
        }
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
            console.log(`Deleting film with ID: ${filmCopyId}`); 
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
});