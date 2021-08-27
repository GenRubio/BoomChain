<?php

namespace App\Models;

use Backpack\CRUD\app\Models\Traits\CrudTrait;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Foundation\Auth\User as Authenticatable;

class Usuario extends Authenticatable
{
    use CrudTrait;

    /*
    |--------------------------------------------------------------------------
    | GLOBAL VARIABLES
    |--------------------------------------------------------------------------
    */

    protected $table = 'usuarios';
    // protected $primaryKey = 'id';
    // public $timestamps = false;
    protected $guarded = ['id'];
    protected $fillable = [
        'nombre',
        'password',
        'token_uid',
        'avatar',
        'colores',
        'email',
        'edad',
        'ip_registro',
        'ip_actual',
        'oro',
        'admin',
        'bocadillo',
        'hobby_1',
        'hobby_2',
        'hobby_3',
        'deseo_1',
        'deseo_2',
        'deseo_3',
        'votos_sexy',
        'votos_legal',
        'votos_simpatico',
        'votos_restantes',
        'votos_recarga',
        'besos_enviados',
        'besos_recibidos',
        'jugos_enviados',
        'jugos_recibidos',
        'flores_enviadas',
        'flores_recibidas',
        'uppers_enviados',
        'uppers_recibidos',
        'cocos_enviados',
        'cocos_recibidos',
        'rings_ganados',
        'senderos_ganados',
        'puntos_cocos',
        'puntos_ninja',
        'ultima_conexion',
        'fecha_registro',
        'metamask'
    ];
    // protected $hidden = [];
    // protected $dates = [];

    /*
    |--------------------------------------------------------------------------
    | FUNCTIONS
    |--------------------------------------------------------------------------
    */

    /*
    |--------------------------------------------------------------------------
    | RELATIONS
    |--------------------------------------------------------------------------
    */

    /*
    |--------------------------------------------------------------------------
    | SCOPES
    |--------------------------------------------------------------------------
    */

    /*
    |--------------------------------------------------------------------------
    | ACCESSORS
    |--------------------------------------------------------------------------
    */

    /*
    |--------------------------------------------------------------------------
    | MUTATORS
    |--------------------------------------------------------------------------
    */
}
