<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\UserDatosUpdateRequest;
use App\Http\Requests\UsuarioRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;
use Illuminate\Support\Facades\Route;

class UserDataCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    protected $user_id;

    public function setup()
    {
        CRUD::setModel(\App\Models\Usuario::class);
        $this->setRoute();
        $this->breadCrumbs();
        $this->filterList();
        CRUD::setEntityNameStrings('personaje', 'personaje');
    }

    private function setRoute()
    {
        $this->user_id = Route::current()->parameter('user_id');

        $this->crud->setRoute(
            "admin/usuario/"
                . $this->user_id . "/user-data"
        );
    }
    private function breadCrumbs()
    {
        $this->data['breadcrumbs'] = [
            trans('backpack::crud.admin') => backpack_url('dashboard'),

            'Usuarios' => backpack_url('usuario'),
            'Personaje' => backpack_url("usuario/"
                . $this->user_id
                . "/personaje"),
            trans('backpack::crud.list') => false,
        ];
    }

    private function filterList()
    {
        $this->crud->addClause('where', 'id', $this->user_id);
    }

    protected function setupListOperation()
    {
        $this->crud->removeButton('create');
        $this->crud->removeButton('delete');

        $this->crud->addColumn([
            'name' => 'nombre',
            'label' => 'Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'bocadillo',
            'label' => 'Descripcion',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'edad',
            'label' => 'Color ficha',
            'type'    => 'select_from_array',
            'options' => config('app.ficha_colores'),
        ]);
    }
    protected function basicFieldsCreate()
    {
        $this->crud->addFields([
            [
                'name' => 'nombre',
                'label' => 'Nombre',
                'type' => 'text',
                'attributes' => [
                    'readonly'    => 'readonly',
                  ],
            ],
            [
                'name' => 'edad',
                'label' => 'Color ficha',
                'type' => 'select_from_array',
                'options' => config('app.ficha_colores'),
            ],
            [
                'name' => 'bocadillo',
                'label' => 'Descripcion',
                'type' => 'text'
            ],
            [
                'name' => 'hobby_1',
                'label' => 'Hobby 1',
                'type' => 'text'
            ],
            [
                'name' => 'hobby_2',
                'label' => 'Hobby 2',
                'type' => 'text'
            ],
            [
                'name' => 'hobby_3',
                'label' => 'Hobby 3',
                'type' => 'text'
            ],
            [
                'name' => 'deseo_1',
                'label' => 'Deseo 1',
                'type' => 'text'
            ],
            [
                'name' => 'deseo_2',
                'label' => 'Deseo 2',
                'type' => 'text'
            ],
            [
                'name' => 'deseo_3',
                'label' => 'Deseo 3',
                'type' => 'text'
            ],
            [
                'name' => 'votos_sexy',
                'label' => 'Votos sexy',
                'type' => 'number'
            ],
            [
                'name' => 'votos_legal',
                'label' => 'Votos legal',
                'type' => 'number'
            ],
            [
                'name' => 'votos_simpatico',
                'label' => 'Votos simpatico',
                'type' => 'number'
            ],
            [
                'name' => 'besos_enviados',
                'label' => 'Besos enviados',
                'type' => 'number'
            ],
            [
                'name' => 'besos_recibidos',
                'label' => 'Besos recibidos',
                'type' => 'number'
            ],
            [
                'name' => 'jugos_enviados',
                'label' => 'Jugos enviados',
                'type' => 'number'
            ],
            [
                'name' => 'jugos_recibidos',
                'label' => 'Jugos recibidos',
                'type' => 'number'
            ],
            [
                'name' => 'flores_enviadas',
                'label' => 'Flores enviados',
                'type' => 'number'
            ],
            [
                'name' => 'flores_recibidas',
                'label' => 'Flores recibidas',
                'type' => 'number'
            ],
            [
                'name' => 'uppers_enviados',
                'label' => 'Uppers enviados',
                'type' => 'number'
            ],
            [
                'name' => 'uppers_recibidos',
                'label' => 'Uppers recibidos',
                'type' => 'number'
            ],
            [
                'name' => 'cocos_enviados',
                'label' => 'Cocos enviados',
                'type' => 'number'
            ],
            [
                'name' => 'cocos_recibidos',
                'label' => 'Cocos recibidos',
                'type' => 'number'
            ],
            [
                'name' => 'rings_ganados',
                'label' => 'Rings ganados',
                'type' => 'number'
            ],
            [
                'name' => 'senderos_ganados',
                'label' => 'Senderos ganados',
                'type' => 'number'
            ],
            [
                'name' => 'puntos_cocos',
                'label' => 'Puntos coco',
                'type' => 'number'
            ],
            [
                'name' => 'puntos_ninja',
                'label' => 'Puntos ninja',
                'type' => 'number'
            ],
        ]);
    }
    protected function setupCreateOperation()
    {
        CRUD::setValidation(UserDatosUpdateRequest::class);

        $this->basicFieldsCreate();
    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
