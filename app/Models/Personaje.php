<?php

namespace App\Models;

use Backpack\CRUD\app\Models\Traits\CrudTrait;
use Illuminate\Database\Eloquent\Model;

class Personaje extends Model
{
    use CrudTrait;

    /*
    |--------------------------------------------------------------------------
    | GLOBAL VARIABLES
    |--------------------------------------------------------------------------
    */

    protected $table = 'personajes';
    // protected $primaryKey = 'id';
    // public $timestamps = false;
    protected $guarded = ['id'];
    protected $fillable = [
        'usuario_id',
        'principal',
        'avatar_id',
        'color_1',
        'color_2',
        'color_3',
        'color_4',
        'color_5',
        'color_6',
        'color_7',
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

    public function getColor1Attribute(){
        return "#" . $this->attributes['color_1'];
    }
    public function getColor2Attribute(){
        return "#" . $this->attributes['color_2'];
    }
    public function getColor3Attribute(){
        return "#" . $this->attributes['color_3'];
    }
    public function getColor4Attribute(){
        return "#" . $this->attributes['color_4'];
    }
    public function getColor5Attribute(){
        return "#" . $this->attributes['color_5'];
    }
    public function getColor6Attribute(){
        return "#" . $this->attributes['color_6'];
    }
    public function getColor7Attribute(){
        return "#" . $this->attributes['color_7'];
    }

    /*
    |--------------------------------------------------------------------------
    | MUTATORS
    |--------------------------------------------------------------------------
    */

    public function setColor1Attribute($value){
        $this->attributes['color_1'] = strtoupper(str_replace("#", "", $value));
    }
    public function setColor2Attribute($value){
        $this->attributes['color_2'] = strtoupper(str_replace("#", "", $value));
    }
    public function setColor3Attribute($value){
        $this->attributes['color_3'] = strtoupper(str_replace("#", "", $value));
    }
    public function setColor4Attribute($value){
        $this->attributes['color_4'] = strtoupper(str_replace("#", "", $value));
    }
    public function setColor5Attribute($value){
        $this->attributes['color_5'] = strtoupper(str_replace("#", "", $value));
    }
    public function setColor6Attribute($value){
        $this->attributes['color_6'] = strtoupper(str_replace("#", "", $value));
    }
    public function setColor7Attribute($value){
        $this->attributes['color_7'] = strtoupper(str_replace("#", "", $value));
    }
}
