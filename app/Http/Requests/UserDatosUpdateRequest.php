<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UserDatosUpdateRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return backpack_auth()->check();
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        return [
            'edad' => 'required',
            'bocadillo' => 'required',
            'hobby_1' => 'required',
            'hobby_2' => 'required',
            'hobby_3' => 'required',
            'deseo_1' => 'required',
            'deseo_2' => 'required',
            'deseo_3' => 'required',
            'votos_sexy' => 'required',
            'votos_legal' => 'required',
            'votos_simpatico' => 'required',
            'besos_enviados' => 'required',
            'besos_recibidos' => 'required',
            'jugos_enviados' => 'required',
            'jugos_recibidos' => 'required',
            'flores_enviadas' => 'required',
            'flores_recibidas' => 'required',
            'uppers_enviados' => 'required',
            'uppers_recibidos' => 'required',
            'cocos_enviados' => 'required',
            'cocos_recibidos' => 'required',
            'rings_ganados' => 'required',
            'senderos_ganados' => 'required',
            'puntos_cocos' => 'required',
            'puntos_ninja' => 'required',
        ];
    }
}
