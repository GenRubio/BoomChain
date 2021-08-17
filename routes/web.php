<?php

use App\Http\Controllers\Auth\MetamaskController;
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

Route::get('/', function () {
    return view('pages.home');
})->name('home');

Route::prefix('metamask')->group(function () {
    Route::post('/', [MetamaskController::class, 'login'])->name('metamask.login');
});

Route::get('/log-out', [PlayController::class, 'logOut'])->name('logOut');

Route::prefix('launcher')->group(function () {
    Route::get('/play', [PlayController::class, 'index'])->name('launcher.play');
});

Route::middleware('auth')->group(function () { 
});