<?php

use Illuminate\Support\Facades\Route;

Route::group([
    'prefix'     => config('backpack.base.route_prefix', 'admin'),
    'middleware' => array_merge(
        (array) config('backpack.base.web_middleware', 'web'),
        (array) config('backpack.base.middleware_key', 'admin')
    ),
    'namespace'  => 'App\Http\Controllers\Admin',
], function () { 

    Route::crud('usuario', 'UsuarioCrudController');

    Route::group(['prefix' => 'usuario/{user_id}'], function () {
        Route::crud('user-data', 'UserDataCrudController');
        Route::crud('personajes', 'PersonajeCrudController');
        Route::crud('islas', 'IslaCrudController');
    });
    Route::crud('escenarios-publico', 'EscenariosPublicoCrudController');
  
    Route::crud('catalago-objeto', 'CatalagoObjetoCrudController');
    Route::crud('catalago-planta', 'CatalagoPlantaCrudController');
});