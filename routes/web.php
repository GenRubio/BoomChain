<?php

use App\Http\Controllers\Auth\MetamaskController;
use App\Http\Controllers\Dashboard\DashboardController;
use App\Http\Controllers\Home\HomeController;
use App\Http\Controllers\Launcher\PlayController;
use Backpack\CRUD\app\Http\Controllers\Auth\LoginController;
use Illuminate\Support\Facades\Route;

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/', [HomeController::class, 'index'])->name('home');

Route::prefix('metamask')->group(function () {
    Route::post('/', [MetamaskController::class, 'index'])->name('metamask.index');
    Route::post('/login-game', [MetamaskController::class, 'loginGame'])->name('metamask.login');
    Route::post('/login-web', [MetamaskController::class, 'loginWeb'])->name('metamask.login.web');
    Route::post('/register', [MetamaskController::class, 'register'])->name('metamask.register');
    Route::get('/log-out', [MetamaskController::class, 'logOut'])->name('user.logout');
});

Route::get('/log-out', [PlayController::class, 'logOut'])->name('logOut');

Route::prefix('launcher')->group(function () {
    Route::get('/play', [PlayController::class, 'index'])->name('launcher.play');
});

Route::middleware('auth')->group(function () {
    Route::get('/dashboard', [DashboardController::class, 'index'])->name('dashboard');
});
