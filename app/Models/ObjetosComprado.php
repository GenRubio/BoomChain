<?php

namespace App\Models;

use Backpack\CRUD\app\Models\Traits\CrudTrait;
use Illuminate\Database\Eloquent\Model;

class ObjetosComprado extends Model
{
    use CrudTrait;

    /*
    |--------------------------------------------------------------------------
    | GLOBAL VARIABLES
    |--------------------------------------------------------------------------
    */

    protected $table = 'objetos_comprados';
    // protected $primaryKey = 'id';
    // public $timestamps = false;
    protected $guarded = ['id'];
    protected $fillable = [
        'objeto_id',
        'usuario_id',
        'sala_id',
        'posX',
        'posY',
        'colores_hex',
        'colores_rgb',
        'rotation',
        'tam',
        'espacio_ocupado',
        'planta_agua',
        'planta_sol',
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
    public function objeto(){
        return $this->hasOne(CatalagoObjeto::class, 'id', 'objeto_id');
    }
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
    public function setColoresHexAttribute($value = null){
        if ($value){
            $this->attributes['colores_hex'] = $value;
        }else{
            $this->attributes['colores_hex'] = $this->objeto->colores_hex;
        } 
    }

    public function setColoresRgbAttribute($value = null){
        if ($value){
            $this->attributes['colores_rgb'] = $value;
        }else{
            $this->attributes['colores_rgb'] = $this->objeto->colores_rgb;
        } 
    }

    public function setEspacioOcupadoAttribute($value = null){
        if ($value){
            $this->attributes['espacio_ocupado'] = $value;
        }else{
            $this->attributes['espacio_ocupado'] = "";
        } 
    }
}
