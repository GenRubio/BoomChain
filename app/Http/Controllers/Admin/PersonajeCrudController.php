<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\PersonajeRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;
use Illuminate\Support\Facades\Route;

class PersonajeCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    protected $user_id;
    
    public function setup()
    {
        CRUD::setModel(\App\Models\Personaje::class);
        $this->setRoute();
        $this->breadCrumbs();
        $this->filterList();
        CRUD::setEntityNameStrings('personaje', 'personajes');
    }

    private function setRoute()
    {
        $this->user_id = Route::current()->parameter('user_id');

        $this->crud->setRoute(
            "admin/usuario/"
                . $this->user_id . "/personajes"
        );
    }
    private function breadCrumbs()
    {
        $this->data['breadcrumbs'] = [
            trans('backpack::crud.admin') => backpack_url('dashboard'),

            'Usuarios' => backpack_url('usuario'),
            'Personaje' => backpack_url("usuario/"
                . $this->user_id
                . "/personajes"),
            trans('backpack::crud.list') => false,
        ];
    }

    private function filterList()
    {
        $this->crud->addClause('where', 'usuario_id', $this->user_id);
    }

    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'avatar_id',
            'label' => 'Personaje',
            'type'    => 'select_from_array',
            'options' => config('app.personajes'),
        ]);
        $this->crud->addColumn([
            'name' => 'principal',
            'label' => 'Principal',
            'type' => 'check'
        ]);
    }

    protected function basicFields()
    {
        $this->crud->addFields([
            [
                'name' => 'avatar_id',
                'label' => 'Personaje',
                'type' => 'select_from_array',
                'options' => config('app.personajes'),
            ],
            [
                'name' => 'usuario_id',
                'type' => 'hidden',
                'value' => $this->user_id
            ],
            [
                'name'    => 'color_1',
                'label'   => 'Color 1',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name'    => 'color_2',
                'label'   => 'Color 2',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name'    => 'color_3',
                'label'   => 'Color 3',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name'    => 'color_4',
                'label'   => 'Color 4',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name'    => 'color_5',
                'label'   => 'Color 5',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name'    => 'color_6',
                'label'   => 'Color 6',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name'    => 'color_7',
                'label'   => 'Color 7',
                'type'    => 'color',
                'default' => '#FFFFFF',
            ],
            [
                'name' => 'principal',
                'label' => 'Principal',
                'type' => 'checkbox',
            ],
        ]);
    }

    
    protected function setupCreateOperation()
    {
        CRUD::setValidation(PersonajeRequest::class);

        $this->basicFields();

    }

    
    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
