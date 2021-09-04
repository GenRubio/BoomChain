<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\CatalagoPlantaRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;


class CatalagoPlantaCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

 
    public function setup()
    {
        CRUD::setModel(\App\Models\CatalagoPlanta::class);
        CRUD::setRoute(config('backpack.base.route_prefix') . '/catalago-planta');
        CRUD::setEntityNameStrings('catalago planta', 'catalago plantas');
    }

    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'objeto',
            'label' => 'Nombre objeto',
            'type' => 'relationship',
            'attribute' => 'titulo',
            'model'     => App\Models\CatalagoObjeto::class,
        ]);
        $this->crud->addColumn([
            'name' => 'creation_time',
            'label' => 'Tiempo de creacion /h',
            'type' => 'number'
        ]);
        $this->crud->addColumn([
            'name' => 'limit_riegue_time',
            'label' => 'Tiempo limite para regar /h',
            'type' => 'number'
        ]);
    }

    protected function basicFields()
    {
        $this->crud->addFields([

            [
                'label' => 'Catalago objeto',
                'type' => 'select2',
                'name' => 'catalago_objeto_id',
                'model'     => "App\Models\CatalagoObjeto",
                'attribute' => 'titulo',
                'options'   => (function ($query) {
                    return $query->where('catalago_objetos.visible', 1)
                        ->where('catalago_objetos.active', 1)
                        ->where(function ($subquery) {
                            $subquery->whereNotIn('id', function ($objetoQuery) {
                                $objetoQuery->select('catalago_objeto_id')->from('catalago_plantas');
                            });
                        })
                        ->get();
                }),
            ],
            [
                'name' => 'creation_time',
                'suffix' => 'h',
                'label' => 'Tiempo de creacion en horas',
                'type' => 'number',
                'default' => 0,
            ],
            [
                'name' => 'limit_riegue_time',
                'suffix' => 'h',
                'label' => 'Tiempo limite para regar en horas',
                'type' => 'number',
                'default' => 0,
            ],
        ]);
    }

    protected function setupCreateOperation()
    {
        CRUD::setValidation(CatalagoPlantaRequest::class);

        $this->basicFields();
    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
