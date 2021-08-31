<?php

use App\Http\Controllers\Auth\MetamaskController;
use App\Http\Controllers\Dashboard\DashboardController;
use App\Http\Controllers\Home\HomeController;
use App\Http\Controllers\Launcher\PlayController;
use Illuminate\Support\Facades\Route;

Route::get('/', [HomeController::class, 'index'])->name('home');

Route::prefix('metamask')->group(function () {
    Route::post('/', [MetamaskController::class, 'index'])->name('metamask.index');
    Route::post('/login-game', [MetamaskController::class, 'loginGame'])->name('metamask.login');
    Route::post('/login-web', [MetamaskController::class, 'loginWeb'])->name('metamask.login.web');
    Route::post('/register', [MetamaskController::class, 'register'])->name('metamask.register');
    Route::get('/log-out', [MetamaskController::class, 'logOut'])->name('user.logout');
});

Route::prefix('launcher')->group(function () {
    Route::get('/play', [PlayController::class, 'index'])->name('launcher.play');
    Route::get('/log-out', [PlayController::class, 'logOut'])->name('logOut');
});

Route::middleware('auth')->group(function () {
    Route::get('/dashboard', [DashboardController::class, 'index'])->name('dashboard');
});
