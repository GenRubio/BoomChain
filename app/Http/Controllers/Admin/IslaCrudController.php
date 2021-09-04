<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\IslaRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;
use Illuminate\Support\Facades\Route;

class IslaCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    protected $user_id;
    
    public function setup()
    {
        CRUD::setModel(\App\Models\Isla::class);
        $this->setRoute();
        $this->breadCrumbs();
        $this->filterList();
        CRUD::setEntityNameStrings('isla', 'islas');
    }

    private function setRoute()
    {
        $this->user_id = Route::current()->parameter('user_id');

        $this->crud->setRoute(
            "admin/usuario/"
                . $this->user_id . "/islas"
        );
    }
    private function breadCrumbs()
    {
        $this->data['breadcrumbs'] = [
            trans('backpack::crud.admin') => backpack_url('dashboard'),

            'Usuarios' => backpack_url('usuario'),
            'Islas' => backpack_url("usuario/"
                . $this->user_id
                . "/islas"),
            trans('backpack::crud.list') => false,
        ];
    }

    private function filterList()
    {
        $this->crud->addClause('where', 'CreadorID', $this->user_id);
    }

    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'nombre',
            'label' => 'Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'descripcion',
            'label' => 'Descripcion',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'modelo',
            'label' => 'Tipo isla',
            'type'    => 'select_from_array',
            'options' => config('app.islas'),
        ]);
    }
    protected function basicFields()
    {
        $this->crud->addFields([
            [
                'name' => 'nombre',
                'label' => 'Nombre',
                'type' => 'text',
            ],
            [
                'name' => 'descripcion',
                'label' => 'Descripcion',
                'type' => 'text',
                'default' => 'Haz uso de este espacio para describir tu isla!',
            ],
            [
                'name' => 'modelo',
                'label' => 'Tipo isla',
                'type' => 'select_from_array',
                'options' => config('app.islas'),
            ],
            [
                'name' => 'CreadorID',
                'type' => 'hidden',
                'value' => $this->user_id
            ],
        ]);
    }
    protected function setupCreateOperation()
    {
        CRUD::setValidation(IslaRequest::class);

        $this->basicFields();
    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
