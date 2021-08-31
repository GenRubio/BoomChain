<?php

namespace App\Models;

use Backpack\CRUD\app\Models\Traits\CrudTrait;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Support\Facades\Hash;

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
        'edad',
        'ip_registro',
        'ip_actual',
        'fecha_registro',
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

    public function personajes(){
        return $this->hasMany(Personaje::class, 'usuario_id', 'id')->where('principal', 0);
    }

    public function personaje(){
        return $this->hasOne(Personaje::class, 'usuario_id', 'id')->where('principal', 1)->limit(1);
    }

    public function islas(){
        return $this->hasMany(Isla::class, 'CreadorID', 'id');
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

    public function setAvatarAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['avatar'] = 1;
        } else {
            $this->attributes['avatar'] = $value;
        }
    }

    public function setColoresAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['colores'] = 'B88A5CFF99000099CC0099CCE31709FFFFFF336666';
        } else {
            $this->attributes['colores'] = $value;
        }
    }

    public function setEdadAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['edad'] = 0;
        } else {
            $this->attributes['edad'] = $value;
        }
    }

    public function setIpRegistroAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['ip_registro'] = $this->getSessionIp();
        } else {
            $this->attributes['ip_registro'] = $value;
        }
    }

    public function setIpActualAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['ip_actual'] = $this->getSessionIp();
        } else {
            $this->attributes['ip_actual'] = $value;
        }
    }

    public function setUltimaConexionAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['ultima_conexion'] = date('Y-m-d H:i:s');
        } else {
            $this->attributes['ultima_conexion'] = $value;
        }
    }

    public function setFechaRegistroAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['fecha_registro'] = date('Y-m-d H:i:s');
        } else {
            $this->attributes['fecha_registro'] = $value;
        }
    }

    public function setPasswordAttribute($value = null)
    {
        $this->attributes['password'] = Hash::make($value);
    }

    public function setTokenUidAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['token_uid'] = $this->createTokenUID();
        } else {
            $this->attributes['token_uid'] = $value;
        }
    }

    public function setNombreAttribute($value = null)
    {
        if ($value == null) {
            $this->attributes['nombre'] = $this->createNameUID();
        } else {
            $this->attributes['nombre'] = $value;
        }
    }

    private function createTokenUID()
    {
        $uid = uniqid('token_');
        while (Usuario::where('token_uid', $uid)->first() != null) {
            $uid = uniqid('token_');
        }
        return $uid;
    }

    private function createNameUID()
    {
        $uid = uniqid();
        while (Usuario::where('nombre', $uid)->first() != null) {
            $uid = uniqid();
        }
        return $uid;
    }

    private function getSessionIp()
    {
        $methods = array(
            'HTTP_CLIENT_IP',
            'HTTP_X_FORWARDED_FOR',
            'HTTP_X_FORWARDED',
            'HTTP_X_CLUSTER_CLIENT_IP',
            'HTTP_FORWARDED_FOR',
            'HTTP_FORWARDED',
            'REMOTE_ADDR'
        );
        foreach ($methods as $key) {
            if (array_key_exists($key, $_SERVER) === true) {
                foreach (explode(',', $_SERVER[$key]) as $ip) {
                    $ip = trim($ip);
                    if (filter_var($ip, FILTER_VALIDATE_IP, FILTER_FLAG_NO_PRIV_RANGE | FILTER_FLAG_NO_RES_RANGE) !== false) {
                        return $ip;
                    }
                }
            }
        }
        return request()->ip();
    }
}
